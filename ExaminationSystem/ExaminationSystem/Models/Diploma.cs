using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Models
{
    [Table("Diplomas")]
    public class Diploma : BaseEntity
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public int TotalQuizCount { get; set; }

        public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;

        #region Admin Relationship
        public Guid AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        public virtual Admin Admin { get; set; }
        #endregion

        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public virtual ICollection<StudentDiploma> StudentEnrollments { get; set; } = new List<StudentDiploma>();
    }
}
