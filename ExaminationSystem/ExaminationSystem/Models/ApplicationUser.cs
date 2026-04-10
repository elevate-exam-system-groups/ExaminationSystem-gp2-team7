using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ExaminationSystem.Models
{
    public abstract class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string UserType { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public int FailedLoginAttempts { get; set; } = 0;

        public DateTime? LockoutUntil { get; set; }

        // OTP for Email Verification
        public string? EmailOtpCode { get; set; }  // Hashed OTP (6 digits)

        public DateTime? EmailOtpExpiresAt { get; set; }  // 10 minutes TTL

        public int EmailOtpAttempts { get; set; } = 0;  // Failed attempts

        public DateTime? EmailOtpLockedUntil { get; set; }  // Locked after 5 attempts

        // Password Reset Token
        public string? ResetToken { get; set; }  // JWT or UUID

        public DateTime? ResetTokenExpiresAt { get; set; }  // 15 minutes TTL

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
