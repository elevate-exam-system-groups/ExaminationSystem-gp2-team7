using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.CreateQuestion;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    /// <summary>
    /// أمر تعديل سؤال MCQ — نفس الـ payload بتاع Create + الـ QuestionId
    /// </summary>
    public record UpdateQuestionCommand : IRequest<Result<bool>>
    {
        /// <summary>
        /// ID السؤال اللي هنعدله (من الـ URL)
        /// </summary>
        public Guid QuestionId { get; init; }

        /// <summary>
        /// نص السؤال الجديد
        /// </summary>
        public string Text { get; init; } = default!;

        /// <summary>
        /// الاختيارات الجديدة (replace كامل)
        /// </summary>
        public List<OptionDto> Options { get; init; } = [];

        /// <summary>
        /// شرح الإجابة الصح
        /// </summary>
        public string? Explanation { get; init; }
    }
}
