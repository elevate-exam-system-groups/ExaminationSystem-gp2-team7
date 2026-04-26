namespace ExaminationSystem.Features.AnswerQuestions
{
    public class AnswerQuestionRequestDTO
    {
        public Guid question_id { get; set; }
        public Guid selected_option_id { get; set; }
    }
}
