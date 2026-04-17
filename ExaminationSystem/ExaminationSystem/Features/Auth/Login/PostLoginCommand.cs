using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Auth.Login
{
    

    public record PostLoginCommand(LoginDTO LoginDTO) : IRequest<Result<LoginResponse>>;
}
