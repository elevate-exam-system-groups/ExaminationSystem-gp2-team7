using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public class SubmitQuizHandler : IRequestHandler<SubmitQuizCommand, Result<SubmitQuizResponse>>
    {
        private readonly ApplicationDbContext _context;

        public SubmitQuizHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<SubmitQuizResponse>> Handle(
            SubmitQuizCommand request, CancellationToken cancellationToken)
        {
            // 1. جيب المحاولة
            var attempt = await _context.Attempts
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken);

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
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == attempt.QuizId, cancellationToken);

            // 5. احسب لو الوقت خلص
            var deadline = attempt.StartTime.AddMinutes(quiz.DurationMinutes);
            var isTimedOut = DateTime.UtcNow > deadline;
            var newStatus = isTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;

            // 6. عدد الأسئلة الكلي
            var totalQuestions = await _context.Questions
                .CountAsync(q => q.QuizId == quiz.Id, cancellationToken);

            // 7. عدد الإجابات الصح
            int correctAnswers;
            if (isTimedOut)
            {
                correctAnswers = await _context.Answers
                    .CountAsync(a =>
                        a.AttemptId == attempt.Id
                        && a.IsCorrect
                        && a.SubmittedAt <= deadline, cancellationToken);
            }
            else
            {
                correctAnswers = await _context.Answers
                    .CountAsync(a =>
                        a.AttemptId == attempt.Id
                        && a.IsCorrect, cancellationToken);
            }

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

            await _context.SaveChangesAsync(cancellationToken);

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
