using System;

namespace ExaminationSystem.Models
{
    
    public abstract partial class ApplicationUser
    {
      
        public string? ResetToken { get; set; }

    
        public DateTime? ResetTokenExpiresAt { get; set; }
    }
}
