using System;

namespace ExaminationSystem.Models
{
    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion()
        {
            QuestionType = "TrueFalse";
        }

        public bool CorrectAnswer { get; set; }
    }
}
