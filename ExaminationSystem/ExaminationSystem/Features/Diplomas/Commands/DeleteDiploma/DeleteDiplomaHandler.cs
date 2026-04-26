using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.DeleteDiploma
{
    public class DeleteDiplomaHandler(IGenericRepository<Diploma> _genericRepo,
                                      IUnitOfWork _unitOfWork)
               : IRequestHandler<DeleteDiplomaCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _genericRepo.GetByIdAsync(request.Id);

            if (diploma is null)
                return Result<string>.NotFound("Diploma not found");

            if (diploma.Status == DiplomaStatus.Published &&
                diploma.StudentEnrollments.Any())
            {
                return Result<string>.Conflict("Published diploma with active enrollments cannot be deleted");
            }

            _genericRepo.Remove(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Diploma deleted successfully");
        }
    }
}
