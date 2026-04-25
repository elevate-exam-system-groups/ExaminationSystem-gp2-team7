using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizHandler(IUnitOfWork unitOfWork, UpdateQuizValidator validator) 
        : IRequestHandler<UpdateQuizCommand, Result<UpdateQuizResponse>>
    {
        public async Task<Result<UpdateQuizResponse>> Handle(UpdateQuizCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateUpdateQuizCommand(
                command, cancellationToken);
            if (validationResult.error is not null)
            {
                return validationResult.error;
            }

            var quiz = validationResult.quiz;
            // Update Quiz
            quiz!.Title = command.Title;
            quiz.DurationMinutes = command.DurationMinutes;
            quiz.PassScore = command.PassScore ?? 60m;
            quiz.Instructions = command.Instructions;
            quiz.UpdatedAt = DateTime.UtcNow;
            quiz.UpdatedBy = command.AdminId;

            // Save changes
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Map response
            var response = new UpdateQuizResponse
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                DiplomaId = quiz.DiplomaId,
                DurationMinutes = quiz.DurationMinutes,
                PassScore = quiz.PassScore,
                MaxAttempts = quiz.MaxAttempts,
                Instructions = quiz.Instructions,
                Status = quiz.Status
            };

            return Result<UpdateQuizResponse>.Success(response, StatusCodes.Status200OK);

        }

        private async Task<(Result<UpdateQuizResponse>? error, Quiz? quiz)> ValidateUpdateQuizCommand(
            UpdateQuizCommand command, CancellationToken cancellationToken)
        {
            // Validation

            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors);
                return (Result<UpdateQuizResponse>.Failure(
                    errors, StatusCodes.Status422UnprocessableEntity), null);
            };


            // Verify Quiz exists
            var quiz = await unitOfWork.GetRepository<Quiz>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == command.QuizId, cancellationToken);
            if (quiz == null)
            {
                return (Result<UpdateQuizResponse>.Failure(
                    "Quiz not found.", StatusCodes.Status404NotFound), null);
            }

            return (null, quiz);
        }
    }
}
