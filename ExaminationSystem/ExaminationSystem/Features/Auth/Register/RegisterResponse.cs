namespace ExaminationSystem.Features.Auth.Register
{

    public class RegisterResponse
    {
        public string email { get; set; } = default!;
        public string full_name { get; set; } = default!;
        public string token { get; set; } = default!;
    }
}
