using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Models
{
    [Table("Students")]
    public class Student : ApplicationUser
    {
        public StudentStatus Status { get; set; } = StudentStatus.Pending;

        public virtual ICollection<StudentDiploma> Enrollments { get; set; } = new List<StudentDiploma>();

        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }
}
