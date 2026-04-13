using System;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Models
{
    [Table("StudentDiplomas")]
    public class StudentDiploma : BaseEntity
    {
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        public decimal Progress { get; set; } = 0m;

        public DateTime? CompletedAt { get; set; }

        #region Student Relationship
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        #endregion

        #region Diploma Relationship
        public Guid DiplomaId { get; set; }
        [ForeignKey(nameof(DiplomaId))]
        public virtual Diploma Diploma { get; set; }
        #endregion
    }
}
