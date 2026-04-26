using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.UpdateQuestion.Helpers;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
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
         
            var questionType = await _reader.GetQuestionTypeAsync(
                request.QuestionId, cancellationToken);

            if (questionType == null)
                return Result<bool>.Failure(
                    "Question not found.", StatusCodes.Status404NotFound);

          
            if (questionType == "TrueFalse")
            {
                var tfQuestion = await _reader.GetTrueFalseQuestionAsync(
                    request.QuestionId, cancellationToken);

                tfQuestion!.QuestionText = request.Text;

                if (request.CorrectAnswer.HasValue)
                    tfQuestion.CorrectAnswer = request.CorrectAnswer.Value;
            }
            else 
            {
                var mcqQuestion = await _reader.GetMcqQuestionAsync(
                    request.QuestionId, cancellationToken);

                mcqQuestion!.QuestionText = request.Text;

                if (request.Options != null && request.Options.Any())
                {
                    
                    var oldOptions = await _reader.GetOptionsByQuestionIdAsync(
                        request.QuestionId, cancellationToken);
                    _reader.RemoveOptions(oldOptions);

                   
                    mcqQuestion.OptionsCount = request.Options.Count;
                    for (int i = 0; i < request.Options.Count; i++)
                    {
                        var optionDto = request.Options[i];
                        _reader.AddOption(new QuestionOption
                        {
                            MCQQuestionId = request.QuestionId,
                            OptionText = optionDto.Text,
                            IsCorrect = optionDto.IsCorrect,
                            OrderIndex = i + 1,
                            Explanation = optionDto.IsCorrect ? request.Explanation : null
                        });
                    }
                }
            }

           
            await _reader.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
