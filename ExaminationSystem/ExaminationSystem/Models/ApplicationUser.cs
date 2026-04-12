using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ExaminationSystem.Models
{
    public abstract class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public string? ProfileImageUrl { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

       
        public string? EmailOtpCode { get; set; } 

        public DateTime? EmailOtpExpiresAt { get; set; }  

        public int EmailOtpAttempts { get; set; } = 0;  

        public DateTime? EmailOtpLockedUntil { get; set; }  

       
        public string? ResetToken { get; set; }  

        public DateTime? ResetTokenExpiresAt { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }
    }
}
