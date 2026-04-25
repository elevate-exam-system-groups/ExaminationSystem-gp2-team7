using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public class CreateQuizHandler(IUnitOfWork unitOfWork, CreateQuizValidator validator) 
        : IRequestHandler<CreateQuizCommand, Result<CreateQuizResponse>>
    {

        public async Task<Result<CreateQuizResponse>> Handle(CreateQuizCommand command, CancellationToken cancellationToken)
        {
            // Validation
            var validationResult = await ValidateCreateQuizCommand(command, cancellationToken);
            if (validationResult is not null)
            {
                return validationResult;
            }

            // Create entity
            var quiz = new Quiz
            {
                Id = Guid.NewGuid(), 
                Title = command.Title,
                DiplomaId = command.DiplomaId,
                DurationMinutes = command.DurationMinutes,
                PassScore = command.PassScore ?? 60m,
                MaxAttempts = command.MaxAttempts,
                Instructions = command.Instructions,
                Status = QuizStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = command.AdminId
            };

            unitOfWork.GetRepository<Quiz>().Add(quiz);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Map response
            var response = new CreateQuizResponse
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

            return Result<CreateQuizResponse>.Success(response, StatusCodes.Status201Created);

        }

        // -------- Helpers ------------

        private async Task<Result<CreateQuizResponse>?> ValidateCreateQuizCommand(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            // Validations
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CreateQuizResponse>.Failure(
                    errors, StatusCodes.Status422UnprocessableEntity);
            }

            // Verify diploma exists
            var diplomaExists = await unitOfWork.GetRepository<Diploma>().AsQueryable()
                .AnyAsync(d => d.Id == request.DiplomaId);
            if (!diplomaExists)
                return Result<CreateQuizResponse>.Failure(
                    "Diploma not found.", StatusCodes.Status404NotFound);

            return null;
        }
    }
}
