namespace ExaminationSystem.Models
{
    public class AuthAppUser : ApplicationUser
    {
        public string RefreshToken { get; set; } = default!;

    }
}
