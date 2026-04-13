using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Answers")]
    public class Answer : BaseEntity
    {
        public bool? StudentAnswer { get; set; }

        public bool IsCorrect { get; set; } = false;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        #region Attempt Relationship
        public Guid AttemptId { get; set; }
        [ForeignKey(nameof(AttemptId))]
        public virtual Attempt Attempt { get; set; }
        #endregion

        #region Question Relationship
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }
        #endregion

        #region Selected Option Relationship
        public Guid? SelectedOptionId { get; set; }
        [ForeignKey(nameof(SelectedOptionId))]
        public virtual Option SelectedOption { get; set; }
        #endregion
    }
}
