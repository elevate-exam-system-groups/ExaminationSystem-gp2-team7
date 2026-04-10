using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Students")]
    public class Student : ApplicationUser
    {
        public Student()
        {
            UserType = "Student";
        }

        public StudentStatus Status { get; set; } = StudentStatus.Pending;

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<StudentDiploma> Enrollments { get; set; } = new List<StudentDiploma>();

        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }

    public enum StudentStatus
    {
        Pending,
        Active,
        Inactive,
        Suspended
    }
}
