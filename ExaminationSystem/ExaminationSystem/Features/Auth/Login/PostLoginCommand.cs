using ExaminationSystem.Common;
using ExaminationSystem.Features.Diplomas.GetDiplomaQuizzes;
using MediatR;

namespace ExaminationSystem.Features.Auth.Login
{
    

    public record PostLoginCommand(LoginDTO LoginDTO) : IRequest<Result<LoginResponse>>;
}
