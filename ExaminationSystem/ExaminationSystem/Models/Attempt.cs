using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Models
{
    [Table("Attempts")]
    public class Attempt : BaseEntity
    {
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime Deadline { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public decimal? Score { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public bool? Passed { get; set; }

        #region Student Relationship
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        #endregion

        #region Quiz Relationship
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }
        #endregion

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
