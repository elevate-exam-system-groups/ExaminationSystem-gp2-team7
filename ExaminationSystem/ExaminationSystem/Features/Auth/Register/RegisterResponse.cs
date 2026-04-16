using ExaminationSystem.Models;

namespace ExaminationSystem.Features.Auth.Register
{

    public class RegisterResponse
    {
        public string email { get; set; } = default!;
        public string full_name { get; set; } = default!;
        public RefreshToken refresh_token { get; set; } = default!;

        public RefreshToken access_token { get; set; } = default!;
    }
}
