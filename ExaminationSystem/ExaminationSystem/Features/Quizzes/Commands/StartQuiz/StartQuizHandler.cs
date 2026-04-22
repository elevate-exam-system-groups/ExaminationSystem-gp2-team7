using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Quizzes.Commands.StartQuiz
{
    public class StartQuizHandler : IRequestHandler<StartQuizCommand, Result<StartQuizResponse>>
    {
        private readonly ApplicationDbContext _dbContext;
        public StartQuizHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<StartQuizResponse>> Handle(StartQuizCommand command, CancellationToken cancellationToken)
        {
            // Create the attempt
            var attempt = new Attempt
            {
                Id = Guid.NewGuid(),
                QuizId = command.QuizId,
                StudentId = command.StudentId,
                Status = AttemptStatus.InProgress,
                StartTime = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = command.StudentId,

            };

            _dbContext.Add(attempt);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var quizInfo = new QuizInfoDto
            {
                Id = attempt.QuizId,
                Title = attempt.Quiz.Title,
                DurationMinutes = attempt.Quiz.DurationMinutes,
                PassScore = attempt.Quiz.PassScore,
                Instructions = attempt.Quiz.Instructions
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
            var mcQuestions = await _dbContext.MultipleChoiceQuestions
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
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Load TrueFalse questions (no options)
            var tfQuestions = await _dbContext.TrueFalseQuestions
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
                .AsNoTracking()
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
