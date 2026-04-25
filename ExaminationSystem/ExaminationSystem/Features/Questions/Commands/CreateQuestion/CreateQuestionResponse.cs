namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    /// <summary>
    /// الرد بعد إنشاء السؤال بنجاح — بيرجع الـ ID بتاعه
    /// </summary>
    public class CreateQuestionResponse
    {
        public Guid QuestionId { get; set; }
    }
}
