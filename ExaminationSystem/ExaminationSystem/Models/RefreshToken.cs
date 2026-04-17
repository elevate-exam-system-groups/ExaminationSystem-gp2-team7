namespace ExaminationSystem.Models
{
    using Microsoft.EntityFrameworkCore;
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        // Computed — no database column, derived on-the-fly
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => RevokedOn == null && !IsExpired;


        // Foreign Key
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
    }

    }
