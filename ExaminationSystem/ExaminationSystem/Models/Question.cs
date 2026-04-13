using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Questions")]
    public abstract class Question : BaseEntity
    {
        public string QuestionText { get; set; }

        public int OrderIndex { get; set; }

        public string QuestionType { get; set; }

        #region Quiz Relationship
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }
        #endregion

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
