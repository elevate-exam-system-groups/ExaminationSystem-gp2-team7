
using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma
{
    public class UpdateDiplomaHandler(IGenericRepository<Diploma> _genericRepo, 
                                      IUnitOfWork _unitOfWork) 
        : IRequestHandler<UpdateDiplomaCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await _genericRepo.GetByIdAsync(request.Id);

            if (diploma is null)
                return Result<string>.NotFound("Diploma not found");

            diploma.Title = request.Title;
            diploma.Description = request.Description;
            diploma.TotalQuizCount = request.TotalQuizCount;

            _genericRepo.Update(diploma);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Diploma updated successfully");
        }
    }
}
