using E_Commerce.Domain.Contracts;
using ExaminationSystem.Common;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz
{
    public class PatchPublishQuizHandler : IRequestHandler<PatchPublishQuizCommand, Result<PublishQuizResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatchPublishQuizHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<PublishQuizResponse>> Handle(PatchPublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.GetRepository<Quiz>();

            var quiz = await quizRepo.GetByIdAsync(request.QuizId);

            if (quiz == null)
                return Result<PublishQuizResponse>.Failure("Quiz not found", StatusCodes.Status404NotFound);

            // already published
            if (quiz.Status == QuizStatus.Published)
                return Result<PublishQuizResponse>.Failure("Quiz already published", StatusCodes.Status404NotFound);

            // load questions
            var questions = quiz.Questions;

            if (questions == null || !questions.Any())
                return Result<PublishQuizResponse>.Failure(
                    "Quiz must have at least one question", StatusCodes.Status422UnprocessableEntity);

            // validate options
            var invalidQuestion = questions
                .Any(q => q.Answers == null || !q.Answers.Any());

            if (invalidQuestion)
                return Result<PublishQuizResponse>.Failure(
                    "All questions must have valid options", StatusCodes.Status422UnprocessableEntity);

            // publish
            quiz.Status = QuizStatus.Published;
            quizRepo.Update(quiz);

            await _unitOfWork.SaveChangesAsync();

            return Result<PublishQuizResponse>.Success(new PublishQuizResponse
            {
                QuizId = quiz.Id,
                Status = "published"
            });
        }
    }
}
