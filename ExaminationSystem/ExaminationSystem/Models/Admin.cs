using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Models
{
    [Table("Admins")]
    public class Admin : ApplicationUser
    {
        public AdminStatus Status { get; set; } = AdminStatus.Active;

        public virtual ICollection<Diploma> CreatedDiplomas { get; set; } = new List<Diploma>();
    }
}
