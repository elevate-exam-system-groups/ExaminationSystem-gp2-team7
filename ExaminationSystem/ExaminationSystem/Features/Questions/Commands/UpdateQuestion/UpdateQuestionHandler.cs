using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.UpdateQuestion.Helpers;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    /// <summary>
    /// Orchestrator: coordinates update flow.
    /// </summary>
    public class UpdateQuestionHandler
        : IRequestHandler<UpdateQuestionCommand, Result<bool>>
    {
        private readonly UpdateQuestionReader _reader;

        public UpdateQuestionHandler(UpdateQuestionReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<bool>> Handle(
            UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            // 1. جيب السؤال
            var question = await _reader.GetQuestionAsync(
                request.QuestionId, cancellationToken);

            if (question == null)
                return Result<bool>.Failure(
                    "Question not found.", StatusCodes.Status404NotFound);

            // 2. جيب الـ Options القديمة (كويري منفصل)
            var oldOptions = await _reader.GetOptionsByQuestionIdAsync(
                request.QuestionId, cancellationToken);

            // 3. امسح الـ Options القديمة
            _reader.RemoveOptions(oldOptions);

            // 4. حدّث نص السؤال
            question.QuestionText = request.Text;
            question.OptionsCount = request.Options.Count;

            // 5. اضيف الـ Options الجديدة
            for (int i = 0; i < request.Options.Count; i++)
            {
                var optionDto = request.Options[i];
                question.Options.Add(new QuestionOption
                {
                    OptionText = optionDto.Text,
                    IsCorrect = optionDto.IsCorrect,
                    OrderIndex = i + 1,
                    Explanation = optionDto.IsCorrect ? request.Explanation : null
                });
            }

            // 6. احفظ
            await _reader.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
