using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Admins")]
    public class Admin : ApplicationUser
    {
        public Admin()
        {
            UserType = "Admin";
        }

        public AdminStatus Status { get; set; } = AdminStatus.Active;

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Diploma> CreatedDiplomas { get; set; } = new List<Diploma>();
    }

    public enum AdminStatus
    {
        Active,
        Inactive,
        Suspended
    }
}
