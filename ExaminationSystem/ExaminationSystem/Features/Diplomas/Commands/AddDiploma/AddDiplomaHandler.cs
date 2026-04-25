using System.Security.Claims;
using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.AddDiploma
{
    public class AddDiplomaHandler(IGenericRepository<Diploma> _genericRepo,
                                   IUnitOfWork _unitOfWork,
                                   IHttpContextAccessor _httpContextAccessor)
        : IRequestHandler<AddDiplomaCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddDiplomaCommand request, CancellationToken cancellationToken)
        {
            var adminId = _httpContextAccessor.HttpContext?
                          .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(adminId))
                return Result<string>.Unauthorized("Admin not authenticated");

            if (!Guid.TryParse(adminId, out var parsedAdminId))
                return Result<string>.Failure("Invalid admin ID format");

            var diploma = new Diploma
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                TotalQuizCount = request.TotalQuizCount,
                Status = DiplomaStatus.Draft,
                AdminId = parsedAdminId
            };

            _genericRepo.Add(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Diploma created successfully");
        }
    }
}
