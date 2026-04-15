using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using MediatR;

namespace ExaminationSystem.Features.Auth.Register
{
    public record PostRegisterCommand(RegisterDTO RegisterDTO) : IRequest<Result<RegisterResponse>>;

}
