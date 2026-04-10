using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Diplomas")]
    public class Diploma : BaseEntity
    {
        public Guid AdminId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;

        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(AdminId))]
        public virtual Admin Admin { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public virtual ICollection<StudentDiploma> StudentEnrollments { get; set; } = new List<StudentDiploma>();
    }

    public enum DiplomaStatus
    {
        Draft,
        Published
    }
}
