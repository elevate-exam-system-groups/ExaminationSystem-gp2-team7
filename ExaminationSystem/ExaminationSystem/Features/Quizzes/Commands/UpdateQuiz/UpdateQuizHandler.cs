using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizHandler : IRequestHandler<UpdateQuizCommand, Result<UpdateQuizResponse>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UpdateQuizValidator _validator;
        public UpdateQuizHandler(
            ApplicationDbContext dbContext, UpdateQuizValidator validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }
        public async Task<Result<UpdateQuizResponse>> Handle(UpdateQuizCommand command, CancellationToken cancellationToken)
        {
            // Validation

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid) { 
                var errors = string.Join(" | ", validationResult.Errors);
                return Result<UpdateQuizResponse>.Failure(
                    errors, StatusCodes.Status422UnprocessableEntity);
            };


            // Verify Quiz exists
            var quiz = await _dbContext.Quizzes.FirstOrDefaultAsync(
                q => q.Id == command.QuizId, cancellationToken);

            if (quiz == null) { 
                return Result<UpdateQuizResponse>.Failure(
                    "Quiz not found.", StatusCodes.Status404NotFound);            
            }
                   

            // Update Quiz
            quiz.Title = command.Title;
            quiz.DurationMinutes = command.DurationMinutes;
            quiz.PassScore = command.PassScore;
            quiz.MaxAttempts = command.MaxAttempts;
            quiz.Instructions = command.Instructions;
            quiz.UpdatedAt = DateTime.UtcNow;
            quiz.UpdatedBy = command.AdminId;

            // Save changes
            await _dbContext.SaveChangesAsync(cancellationToken);

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
    }
}
