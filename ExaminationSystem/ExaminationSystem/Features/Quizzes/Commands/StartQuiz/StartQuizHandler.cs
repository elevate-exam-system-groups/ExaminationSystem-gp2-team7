using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.StartQuiz
{
    public class StartQuizHandler(IUnitOfWork unitOfWork) : IRequestHandler<StartQuizCommand, Result<StartQuizResponse>>
    {
        public async Task<Result<StartQuizResponse>> Handle(StartQuizCommand command, CancellationToken cancellationToken)
        {
            // Fetch the Quiz FIRST (To validate it exists and get duration/info)
            var quiz = await unitOfWork.GetRepository<Quiz>().AsQueryable()
                .Select(q => new { q.Id, q.Title, q.DurationMinutes, q.PassScore, q.Instructions })
                .FirstOrDefaultAsync(q => q.Id == command.QuizId, cancellationToken);

            if (quiz == null)
            {
                return Result<StartQuizResponse>.Failure(
                    "Quiz not found.", StatusCodes.Status404NotFound);
            }

            // Create the attempt
            var attempt = new Attempt
            {
                Id = Guid.NewGuid(),
                QuizId = command.QuizId,
                StudentId = command.StudentId,
                Status = AttemptStatus.InProgress,
                StartTime = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddMinutes(quiz.DurationMinutes),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = command.StudentId
            };

            unitOfWork.GetRepository<Attempt>().Add(attempt);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var quizInfo = new QuizInfoDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                DurationMinutes = quiz.DurationMinutes,
                PassScore = quiz.PassScore,
                Instructions = quiz.Instructions
            };

            // Load questions ordered by OrderIndex
            var questions = await LoadQuestionsAsync(attempt, cancellationToken);

            var response = new StartQuizResponse
            {
                AttemptId = attempt.Id,
                quiz = quizInfo,
                questions = questions,
                StartTime = attempt.StartTime
            };

            return Result<StartQuizResponse>.Success(response, StatusCodes.Status201Created);
        }

        private async Task<List<QuestionDto>> LoadQuestionsAsync(Attempt attempt, CancellationToken cancellationToken)
        {
            // Load MCQ questions with their options(no IsCorrect)
            var mcQuestions = await unitOfWork.GetRepository<MultipleChoiceQuestion>().AsQueryable()
                .Where(q => q.QuizId == attempt.QuizId)
                .OrderBy(q => q.OrderIndex)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.QuestionText,
                    Type = q.QuestionType,
                    OrderIndex = q.OrderIndex,
                    Options = q.Options
                        .Select(o => new OptionDto
                        {
                            Id = o.Id,
                            Text = o.OptionText,
                            OrderIndex = o.OrderIndex
                        }).ToList()
                })
                .ToListAsync(cancellationToken);

            // Load TrueFalse questions (no options)
            var tfQuestions = await unitOfWork.GetRepository<TrueFalseQuestion>().AsQueryable()
                .Where(q => q.QuizId == attempt.QuizId)
                .OrderBy(q => q.OrderIndex)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.QuestionText,
                    Type = q.QuestionType,
                    OrderIndex = q.OrderIndex,
                    Options = null
                })
                .ToListAsync(cancellationToken);

            //  Combine both lists
            //mcQuestions.AddRange(tfQuestions);

            // Merge, then shuffle questions
            var finalQuestions = new List<QuestionDto>(mcQuestions.Concat(tfQuestions));
            Random.Shared.Shuffle(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(finalQuestions));

            return finalQuestions.ToList();
        }
    }
}
