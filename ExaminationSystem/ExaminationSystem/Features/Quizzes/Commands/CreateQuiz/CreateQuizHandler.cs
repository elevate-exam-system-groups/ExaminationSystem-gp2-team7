using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public class CreateQuizHandler : IRequestHandler<CreateQuizCommand, Result<CreateQuizResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly CreateQuizValidator _validator;

        public CreateQuizHandler(ApplicationDbContext context, CreateQuizValidator validator)
        {
            this._context = context;
            this._validator = validator;
        }

        public async Task<Result<CreateQuizResponse>> Handle(CreateQuizCommand command, CancellationToken cancellationToken)
        {
            // Validation
            (bool flowControl, Result<CreateQuizResponse> value) = await ValidateCreateQuizCommand(command, cancellationToken);
            if (!flowControl)
            {
                return value;
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

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync(cancellationToken);

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

        private async Task<(bool flowControl, Result<CreateQuizResponse> value)> ValidateCreateQuizCommand(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            // Validations
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return (flowControl: false, value: Result<CreateQuizResponse>.Failure(errors, StatusCodes.Status422UnprocessableEntity));
            }

            // Verify diploma exists
            var diplomaExists = await _context.Diplomas.AnyAsync(d => d.Id == request.DiplomaId);
            if (!diplomaExists)
                return (flowControl: false, value: Result<CreateQuizResponse>.Failure(
                    "Diploma not found.", StatusCodes.Status404NotFound));
            return (flowControl: true, value: null);
        }
    }
}
