using Microsoft.AspNetCore.Identity;
using System;

namespace ExaminationSystem.Models
{
    
    public partial class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }

        public bool IsDeleted => DeletedAt != null;
    }
}
