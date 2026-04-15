using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Auth.Login
{

    public class LoginResponse
    {
        public string email { get; set; } = default!;
        public string token { get; set; } = default!;
    }

}
