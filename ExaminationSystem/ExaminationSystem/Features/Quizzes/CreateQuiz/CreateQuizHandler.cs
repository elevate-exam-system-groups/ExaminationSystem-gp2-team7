using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
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

        public async Task<Result<CreateQuizResponse>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            // Validations
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<CreateQuizResponse>.Failure(errors, StatusCodes.Status422UnprocessableEntity);
            }

            // Verify diploma exists
            var diplomaExists = await _context.Diplomas.AnyAsync(d => d.Id == request.DiplomaId);
            if (!diplomaExists)
                return Result<CreateQuizResponse>.Failure(
                    "Diploma not found.", StatusCodes.Status404NotFound);
            
            // Create entity
            var quiz = new Quiz
            {
                Title = request.Title,
                DiplomaId = request.DiplomaId,
                DurationMinutes = request.DurationMinutes,
                PassScore = request.PassScore?? 60m,
                MaxAttempts = request.MaxAttempts,
                Instructions = request.Instructions,
                Status = QuizStatus.Draft              
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
    }
}
