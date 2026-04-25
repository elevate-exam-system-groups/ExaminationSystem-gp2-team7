using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    /// <summary>
    /// الأمر اللي بيوصف الـ payload بتاع إضافة سؤال MCQ
    /// </summary>
    public record CreateQuestionCommand : IRequest<Result<CreateQuestionResponse>>
    {
        /// <summary>
        /// ID الكويز اللي هنضيف فيه السؤال (من الـ URL)
        /// </summary>
        public Guid QuizId { get; init; }

        /// <summary>
        /// نص السؤال
        /// </summary>
        public string Text { get; init; } = default!;

        /// <summary>
        /// الاختيارات (لازم 2 على الأقل، واحد بالظبط correct)
        /// </summary>
        public List<OptionDto> Options { get; init; } = [];

        /// <summary>
        /// شرح الإجابة الصح
        /// </summary>
        public string? Explanation { get; init; }
    }

    /// <summary>
    /// شكل كل اختيار في الـ payload
    /// </summary>
    public class OptionDto
    {
        public string Text { get; set; } = default!;
        public bool IsCorrect { get; set; }
    }
}
