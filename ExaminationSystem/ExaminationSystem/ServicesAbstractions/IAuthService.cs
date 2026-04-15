using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using ExaminationSystem.Features.Auth.Register;
using Microsoft.OpenApi.Any;

namespace ExaminationSystem.ServicesAbstractions
{
    public interface IAuthService
    {
        //Task<Result<AnyType>> RegisterAsync(RegisterDTO registerDTO);
        Task<Result<AnyType>> LoginAsync(LoginDTO loginDTO);
        //Task<Result<LoginResponse>> LoginAsync(LoginDTO loginDTO);
    }
}
