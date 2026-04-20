using ExaminationSystem.Common;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using ExaminationSystem.Contracts;
using MediatR;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public class SubmitQuizHandler : IRequestHandler<SubmitQuizCommand, Result<SubmitQuizResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitQuizHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SubmitQuizResponse>> Handle(
            SubmitQuizCommand request, CancellationToken cancellationToken)
        {
           
            var attemptRepo = _unitOfWork.GetRepository<Attempt>();
            var quizRepo = _unitOfWork.GetRepository<Quiz>();
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var answerRepo = _unitOfWork.GetRepository<Answer>();

            
            var attempt = await attemptRepo.GetByIdAsync(request.AttemptId);
            if (attempt == null)
                return Result<SubmitQuizResponse>.Failure("Attempt not found.", 404);

           
            if (attempt.StudentId != request.StudentId)
                return Result<SubmitQuizResponse>.Failure("You are not the owner of this attempt.", 403);

           
            if (attempt.Status != AttemptStatus.InProgress)
                return Result<SubmitQuizResponse>.Failure("This attempt has already been submitted.", 409);

            
            var quiz = await quizRepo.GetByIdAsync(attempt.QuizId);

            
            var deadline = attempt.StartTime.AddMinutes(quiz.DurationMinutes);
            var isTimedOut = DateTime.UtcNow > deadline;
            var newStatus = isTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;

            var totalQuestions = await questionRepo.CountAsync(q => q.QuizId == quiz.Id);

            int correctAnswers;
            if (isTimedOut)
            {
               
                correctAnswers = await answerRepo.CountAsync(a =>
                    a.AttemptId == attempt.Id
                    && a.IsCorrect
                    && a.SubmittedAt <= deadline);
            }
            else
            {
              
                correctAnswers = await answerRepo.CountAsync(a =>
                    a.AttemptId == attempt.Id
                    && a.IsCorrect);
            }

            var score = totalQuestions > 0
                ? (decimal)correctAnswers / totalQuestions * 100
                : 0;

            var passed = score >= quiz.PassScore;

            
            attempt.Status = newStatus;
            attempt.SubmittedAt = DateTime.UtcNow;
            attempt.Score = score;
            attempt.TotalQuestions = totalQuestions;
            attempt.CorrectAnswers = correctAnswers;
            attempt.Passed = passed;

            attemptRepo.Update(attempt);
            await _unitOfWork.SaveChangesAsync();

           
            var response = new SubmitQuizResponse
            {
                AttemptId = attempt.Id,
                Score = score,
                IsPassed = passed,
                Status = newStatus.ToString()
            };

            return Result<SubmitQuizResponse>.Success(response);
        }
    }
}
