using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Attempts")]
    public class Attempt : BaseEntity
    {
        public Guid StudentId { get; set; }

        public Guid QuizId { get; set; }

        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        public DateTime? SubmittedAt { get; set; }

        public decimal? Score { get; set; }

        public int? TotalQuestions { get; set; }

        public int? CorrectAnswers { get; set; }

        public bool? Passed { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

    public enum AttemptStatus
    {
        InProgress,
        Submitted,
        TimedOut
    }
}
