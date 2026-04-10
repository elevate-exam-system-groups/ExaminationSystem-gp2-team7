using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("StudentDiplomas")]
    public class StudentDiploma
    {
        [System.ComponentModel.DataAnnotations.Key]
        public Guid EnrollmentId { get; set; } = Guid.NewGuid();

        public Guid StudentId { get; set; }

        public Guid DiplomaId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        public decimal Progress { get; set; } = 0m;

        public DateTime? CompletedAt { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(DiplomaId))]
        public virtual Diploma Diploma { get; set; }
    }

    public enum EnrollmentStatus
    {
        Active,
        Completed,
        Archived,
        Dropped
    }
}
