using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.CreateQuestion.Helpers;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    /// <summary>
    /// Orchestrator: coordinates validation, business logic, and persistence.
    /// All DB operations are delegated to CreateQuestionReader.
    /// </summary>
    public class CreateQuestionHandler
        : IRequestHandler<CreateQuestionCommand, Result<CreateQuestionResponse>>
    {
        private readonly CreateQuestionReader _reader;

        public CreateQuestionHandler(CreateQuestionReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<CreateQuestionResponse>> Handle(
            CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            // 1. تأكد إن الكويز موجود
            var quiz = await _reader.GetQuizAsync(request.QuizId, cancellationToken);

            if (quiz == null)
                return Result<CreateQuestionResponse>.Failure(
                    "Quiz not found.", StatusCodes.Status404NotFound);

            // 2. حدد الـ OrderIndex (آخر سؤال + 1)
            var lastOrderIndex = await _reader.GetLastOrderIndexAsync(
                request.QuizId, cancellationToken);

            // 3. اعمل السؤال (MCQ)
            var question = new MultipleChoiceQuestion
            {
                QuizId = request.QuizId,
                QuestionText = request.Text,
                QuestionType = "MCQ",
                OrderIndex = lastOrderIndex + 1,
                OptionsCount = request.Options.Count
            };

            // 4. اعمل الاختيارات
            for (int i = 0; i < request.Options.Count; i++)
            {
                var optionDto = request.Options[i];
                question.Options.Add(new Option
                {
                    OptionText = optionDto.Text,
                    IsCorrect = optionDto.IsCorrect,
                    OrderIndex = i + 1,
                    Explanation = optionDto.IsCorrect ? request.Explanation : null
                });
            }

            // 5. اضيف السؤال في الداتابيز
            _reader.AddQuestion(question);

            // 6. حدّث عدد الأسئلة في الكويز
            quiz.TotalQuestionsCache++;

            // 7. احفظ كل التغييرات
            await _reader.SaveChangesAsync(cancellationToken);

            // 8. رجّع الـ ID
            return Result<CreateQuestionResponse>.Success(
                new CreateQuestionResponse { QuestionId = question.Id },
                StatusCodes.Status201Created);
        }
    }
}
