using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Options")]
    public class Option : BaseEntity
    {
        public Guid MCQQuestionId { get; set; }

        public string OptionText { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int OrderIndex { get; set; }

        public string? Explanation { get; set; }

        [ForeignKey(nameof(MCQQuestionId))]
        public virtual MultipleChoiceQuestion MCQQuestion { get; set; }

        public virtual ICollection<Answer> SelectedAnswers { get; set; } = new List<Answer>();

        public virtual ICollection<Answer> CorrectAnswers { get; set; } = new List<Answer>();
    }
}
