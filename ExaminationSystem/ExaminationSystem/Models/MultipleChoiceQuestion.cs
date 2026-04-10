using System;
using System.Collections.Generic;

namespace ExaminationSystem.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public MultipleChoiceQuestion()
        {
            QuestionType = "MCQ";
        }

        public bool AllowMultipleCorrectAnswers { get; set; } = false;

        public int OptionsCount { get; set; } = 0;

        public virtual ICollection<Option> Options { get; set; } = new List<Option>();
    }
}
