using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Options")]
    public class QuestionOption : BaseEntity
    {
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; } = false;

        public int OrderIndex { get; set; }

        public string? Explanation { get; set; }

        #region MCQ Question Relationship
        public Guid MCQQuestionId { get; set; }
        [ForeignKey(nameof(MCQQuestionId))]
        public virtual MultipleChoiceQuestion MCQQuestion { get; set; }
        #endregion

        public virtual ICollection<Answer> SelectedAnswers { get; set; } = new List<Answer>();
    }
}
