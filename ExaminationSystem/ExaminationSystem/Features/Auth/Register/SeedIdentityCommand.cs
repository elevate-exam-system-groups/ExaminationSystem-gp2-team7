using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Auth.Register
{
    public record SeedIdentityCommand : IRequest<Result<RegisterResponse>>;

   
}
