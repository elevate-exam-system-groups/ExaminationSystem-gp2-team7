namespace ExaminationSystem.Models
{
    public partial class ApplicationUser
    {
        // One user → many refresh tokens (multi-device support)
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
