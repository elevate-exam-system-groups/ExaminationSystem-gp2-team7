using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.CreateQuestion.Helpers;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
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
           
            var quiz = await _reader.GetQuizAsync(request.QuizId, cancellationToken);

            if (quiz == null)
                return Result<CreateQuestionResponse>.Failure(
                    "Quiz not found.", StatusCodes.Status404NotFound);

            
            var lastOrderIndex = await _reader.GetLastOrderIndexAsync(
                request.QuizId, cancellationToken);

          
            Question question;

            if (request.QuestionType == "TrueFalse")
            {
                question = new TrueFalseQuestion
                {
                    QuizId = request.QuizId,
                    QuestionText = request.Text,
                    QuestionType = "TrueFalse",
                    OrderIndex = lastOrderIndex + 1,
                    CorrectAnswer = request.CorrectAnswer!.Value
                };
            }
            else 
            {
                var mcqQuestion = new MultipleChoiceQuestion
                {
                    QuizId = request.QuizId,
                    QuestionText = request.Text,
                    QuestionType = "MCQ",
                    OrderIndex = lastOrderIndex + 1,
                    OptionsCount = request.Options!.Count
                };

                for (int i = 0; i < request.Options.Count; i++)
                {
                    var optionDto = request.Options[i];
                    mcqQuestion.Options.Add(new QuestionOption
                    {
                        OptionText = optionDto.Text,
                        IsCorrect = optionDto.IsCorrect,
                        OrderIndex = i + 1,
                        Explanation = optionDto.IsCorrect ? request.Explanation : null
                    });
                }

                question = mcqQuestion;
            }

          
            _reader.AddQuestion(question);

          
            quiz.TotalQuestionsCache++;

           
            await _reader.SaveChangesAsync(cancellationToken);

           
            return Result<CreateQuestionResponse>.Success(
                new CreateQuestionResponse { QuestionId = question.Id },
                StatusCodes.Status201Created);
        }
    }
}
