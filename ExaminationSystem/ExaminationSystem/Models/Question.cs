using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Questions")]
    public abstract class Question : BaseEntity
    {
        public Guid QuizId { get; set; }

        public string QuestionText { get; set; }

        public int OrderIndex { get; set; }

        public string QuestionType { get; set; }

        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
