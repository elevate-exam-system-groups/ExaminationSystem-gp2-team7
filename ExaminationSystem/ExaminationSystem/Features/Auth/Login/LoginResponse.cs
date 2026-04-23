using System.ComponentModel.DataAnnotations;
using ExaminationSystem.Models;

namespace ExaminationSystem.Features.Auth.Login
{

    public class LoginResponse
    {
        public string email { get; set; } = default!;
        public RefreshToken refresh_token { get; set; } = default!;

        public RefreshToken access_token { get; set; } = default!;
    }

}
