using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Answers")]
    public class Answer : BaseEntity
    {
        public Guid AttemptId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? SelectedOptionId { get; set; }

        public bool? StudentAnswer { get; set; }

        public bool? CorrectTrueAnswer { get; set; }

        public bool IsCorrect { get; set; } = false;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(AttemptId))]
        public virtual Attempt Attempt { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }

        [ForeignKey(nameof(SelectedOptionId))]
        public virtual Option SelectedOption { get; set; }
    }
}
