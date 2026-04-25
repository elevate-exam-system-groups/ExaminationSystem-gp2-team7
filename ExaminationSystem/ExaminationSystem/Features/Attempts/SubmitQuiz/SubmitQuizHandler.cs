using ExaminationSystem.Common;
using ExaminationSystem.Features.Attempts.SubmitQuiz.Helpers;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    /// <summary>
    /// Orchestrator: coordinates validation, business logic, and persistence.
    /// All DB operations are delegated to SubmitQuizReader.
    /// </summary>
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
            // 1. جيب المحاولة
            var attempt = await _reader.GetAttemptAsync(request.AttemptId, cancellationToken);

            if (attempt == null)
                return Result<SubmitQuizResponse>.Failure(
                    "Attempt not found.", StatusCodes.Status404NotFound);

            // 2. تأكد إن الطالب هو صاحب المحاولة
            if (attempt.StudentId != request.StudentId)
                return Result<SubmitQuizResponse>.Failure(
                    "You are not the owner of this attempt.", StatusCodes.Status403Forbidden);

            // 3. لو المحاولة اتسلمت قبل كده → ارجع النتيجة القديمة مع 409
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

            // 4. جيب الكويز
            var quiz = await _reader.GetQuizAsync(attempt.QuizId, cancellationToken);

            // 5. احسب لو الوقت خلص
            var deadline = attempt.StartTime.AddMinutes(quiz.DurationMinutes);
            var isTimedOut = DateTime.UtcNow > deadline;
            var newStatus = isTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;

            // 6. عدد الأسئلة الكلي
            var totalQuestions = await _reader.CountQuestionsAsync(quiz.Id, cancellationToken);

            // 7. عدد الإجابات الصح
            var correctAnswers = isTimedOut
                ? await _reader.CountCorrectAnswersBeforeDeadlineAsync(attempt.Id, deadline, cancellationToken)
                : await _reader.CountCorrectAnswersAsync(attempt.Id, cancellationToken);

            // 8. احسب الدرجة
            var score = totalQuestions > 0
                ? (decimal)correctAnswers / totalQuestions * 100
                : 0;

            var passed = score >= quiz.PassScore;

            // 9. حدّث المحاولة
            attempt.Status = newStatus;
            attempt.SubmittedAt = DateTime.UtcNow;
            attempt.Score = score;
            attempt.TotalQuestions = totalQuestions;
            attempt.CorrectAnswers = correctAnswers;
            attempt.Passed = passed;

            await _reader.SaveChangesAsync(cancellationToken);

            // 10. رجّع النتيجة
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
