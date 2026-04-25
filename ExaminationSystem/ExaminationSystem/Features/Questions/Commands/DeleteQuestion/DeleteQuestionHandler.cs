using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.DeleteQuestion.Helpers;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion
{
    /// <summary>
    /// Orchestrator: coordinates delete flow.
    /// </summary>
    public class DeleteQuestionHandler
        : IRequestHandler<DeleteQuestionCommand, Result<bool>>
    {
        private readonly DeleteQuestionReader _reader;

        public DeleteQuestionHandler(DeleteQuestionReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<bool>> Handle(
            DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            // 1. جيب السؤال
            var question = await _reader.GetQuestionAsync(
                request.QuestionId, cancellationToken);

            if (question == null)
                return Result<bool>.Failure(
                    "Question not found.", StatusCodes.Status404NotFound);

            // 2. جيب الكويز بتاعه
            var quiz = await _reader.GetQuizAsync(question.QuizId, cancellationToken);

            // 3. لو الكويز Published → مينفعش تمسح
            if (quiz!.Status == QuizStatus.Published)
                return Result<bool>.Failure(
                    "Cannot delete a question while the quiz is published. Unpublish the quiz first or soft-delete the question.",
                    StatusCodes.Status409Conflict);

            // 4. لو Draft → Soft Delete السؤال + الـ Options
            _reader.SoftDeleteQuestion(question);
            await _reader.SoftDeleteOptionsAsync(question.Id, cancellationToken);

            // 5. حدّث عدد الأسئلة
            quiz.TotalQuestionsCache--;

            // 6. احفظ
            await _reader.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
