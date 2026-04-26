using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.DeleteQuestion.Helpers;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion
{
   
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
            
            var question = await _reader.GetQuestionAsync(
                request.QuestionId, cancellationToken);

            if (question == null)
                return Result<bool>.Failure(
                    "Question not found.", StatusCodes.Status404NotFound);

          
            var quiz = await _reader.GetQuizAsync(question.QuizId, cancellationToken);

           
            if (quiz!.Status == QuizStatus.Published)
                return Result<bool>.Failure(
                    "Cannot delete a question while the quiz is published. Unpublish the quiz first or soft-delete the question.",
                    StatusCodes.Status409Conflict);

         
            _reader.SoftDeleteQuestion(question);
            await _reader.SoftDeleteOptionsAsync(question.Id, cancellationToken);

           
            quiz.TotalQuestionsCache--;

         
            await _reader.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
