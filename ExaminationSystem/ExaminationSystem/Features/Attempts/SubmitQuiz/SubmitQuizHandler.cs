using ExaminationSystem.Common;
using ExaminationSystem.Features.Attempts.SubmitQuiz.Helpers;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public class SubmitQuizHandler : IRequestHandler<SubmitQuizCommand, Result<SubmitQuizResponse>>
    {
        private readonly SubmitQuizReader _reader;

        public SubmitQuizHandler(SubmitQuizReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<SubmitQuizResponse>> Handle(
            SubmitQuizCommand request, CancellationToken cancellationToken)
        {
           
            var attempt = await _reader.GetAttemptAsync(request.AttemptId, cancellationToken);

            if (attempt == null)
                return Result<SubmitQuizResponse>.Failure(
                    "Attempt not found.", StatusCodes.Status404NotFound);

          
            if (attempt.StudentId != request.StudentId)
                return Result<SubmitQuizResponse>.Failure(
                    "You are not the owner of this attempt.", StatusCodes.Status403Forbidden);

          
            if (attempt.Status != AttemptStatus.InProgress)
                return Result<SubmitQuizResponse>.Failure(
                    "This attempt has already been submitted.",
                    StatusCodes.Status409Conflict,
                    new SubmitQuizResponse
                    {
                        AttemptId = attempt.Id,
                        Score = attempt.Score ?? 0,
                        IsPassed = attempt.Passed ?? false,
                        Status = attempt.Status.ToString()
                    });

          
            var quiz = await _reader.GetQuizAsync(attempt.QuizId, cancellationToken);

          
            var deadline = attempt.StartTime.AddMinutes(quiz.DurationMinutes);
            var isTimedOut = DateTime.UtcNow > deadline;
            var newStatus = isTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;

          
            var totalQuestions = await _reader.CountQuestionsAsync(quiz.Id, cancellationToken);

           
            var correctAnswers = isTimedOut
                ? await _reader.CountCorrectAnswersBeforeDeadlineAsync(attempt.Id, deadline, cancellationToken)
                : await _reader.CountCorrectAnswersAsync(attempt.Id, cancellationToken);

            
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

            await _reader.SaveChangesAsync(cancellationToken);

            
            return Result<SubmitQuizResponse>.Success(new SubmitQuizResponse
            {
                AttemptId = attempt.Id,
                Score = score,
                IsPassed = passed,
                Status = newStatus.ToString()
            });
        }
    }
}
