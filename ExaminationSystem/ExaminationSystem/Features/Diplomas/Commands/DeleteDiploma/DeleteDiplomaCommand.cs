using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.DeleteDiploma
{
    public record DeleteDiplomaCommand(Guid Id) : IRequest<Result<string>>;
}
