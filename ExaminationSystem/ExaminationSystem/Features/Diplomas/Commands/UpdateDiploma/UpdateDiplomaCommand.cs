using ExaminationSystem.Common;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma
{
    public record UpdateDiplomaCommand(Guid Id,
                                       string Title,
                                       string? Description,
                                       int TotalQuizCount) 
                : IRequest<Result<string>>;
}
