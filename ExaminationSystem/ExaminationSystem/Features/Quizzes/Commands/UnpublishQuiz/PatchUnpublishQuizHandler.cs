using E_Commerce.Domain.Contracts;
using ExaminationSystem.Common;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.PublishQuiz
{
    public class PatchUnpublishQuizHandler : IRequestHandler<PatchUnpublishQuizCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatchUnpublishQuizHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(PatchUnpublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.GetRepository<Quiz>();
            var attemptRepo = _unitOfWork.GetRepository<Attempt>();

            var quiz = await quizRepo.GetByIdAsync(request.QuizId);

            if (quiz == null)
                return Result<string>.Failure("Quiz not found", StatusCodes.Status404NotFound);

            // check active attempts
            var hasActiveAttempts = (await attemptRepo.GetAllAsync())
                .Any(a => a.QuizId == quiz.Id && a.Status == AttemptStatus.InProgress);

            if (hasActiveAttempts)
                return Result<string>.Failure(
                    "Cannot unpublish quiz with active attempts", StatusCodes.Status409Conflict);

            quiz.Status = QuizStatus.Draft;
            quizRepo.Update(quiz);

            await _unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Unpublished");
        }
    }
}
