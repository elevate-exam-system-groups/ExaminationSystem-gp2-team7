using System;

namespace ExaminationSystem.Models
{
  
    public partial class ApplicationUser
    {
       
        public string? EmailOtpCode { get; set; }

      
        public DateTime? EmailOtpExpiresAt { get; set; }

        
        public int EmailOtpAttempts { get; set; } = 0;

      
        public DateTime? EmailOtpLockedUntil { get; set; }

       
        public int OtpResendCount { get; set; } = 0;

        public DateTime? OtpResendWindowStart { get; set; }
    }
}
