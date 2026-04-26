using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using ExaminationSystem.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AnswerQuestions
{
    public class PostAnswerQuestionHandler : IRequestHandler<PostAswerQuestionCommand, Result<AnsewrQuestionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostAnswerQuestionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AnsewrQuestionResponse>> Handle(PostAswerQuestionCommand request, CancellationToken cancellationToken)
        {
            // 1. Get Attempt

            var attemptRepo = _unitOfWork.GetRepository<Attempt>();
            var attemps = await attemptRepo.GetAllAsync();
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var questions = await questionRepo.GetAllAsync();   

            // 1. Get attempt
            var attempt = attemps.FirstOrDefault(a => a.Id == request.AnswerQuestionDTO.AttemptId);

            if (attempt is null)
            {
                return Result<AnsewrQuestionResponse>.Failure("Attempt not found", StatusCodes.Status404NotFound);
            }

            // 2. Ownership
            if (attempt.Id != request.AnswerQuestionDTO.UserId)
                return Result<AnsewrQuestionResponse>.Failure("Forbidden", StatusCodes.Status403Forbidden);


            // 3. Status check
            if (attempt.Status != AttemptStatus.InProgress)
                return Result<AnsewrQuestionResponse>.Failure("Attempt already submitted", StatusCodes.Status409Conflict);

            // 4. Timer check
            if (attempt.Deadline <= DateTime.UtcNow)
            {
                attempt.Status = AttemptStatus.Submitted;
                attemptRepo.Update(attempt);
                await _unitOfWork.SaveChangesAsync();

                return Result<AnsewrQuestionResponse>.Failure("Time expired", StatusCodes.Status410Gone);
            }



            // 5. Validate question belongs to quiz + include answers
            var question = questions.Include(q => q.Answers)
                .FirstOrDefault(q => q.Id == request.AnswerQuestionDTO.question_id && q.QuizId == attempt.QuizId);

            if (question == null)
                return Result<AnsewrQuestionResponse>.Failure("Invalid question", StatusCodes.Status422UnprocessableEntity);

            // 6. Validate option
            var isValidOption = question.Answers.Any(a => a.Id == request.AnswerQuestionDTO.selected_option_id);
            if (!isValidOption)
                return Result<AnsewrQuestionResponse>.Failure("Invalid option", StatusCodes.Status422UnprocessableEntity);
            // 7. Upsert (update أو insert)
            var existingAnswer = await attemps.FirstOrDefaultAsync(a =>
                    a.Id == request.AnswerQuestionDTO.AttemptId &&
                    a.Quiz.Questions.Any(q => q.Id == request.AnswerQuestionDTO.question_id));


            if (existingAnswer != null)
            {
                var answer = existingAnswer.Answers
                    .FirstOrDefault(a => a.QuestionId == request.AnswerQuestionDTO.question_id);

                if (answer != null)
                {
                    answer.SelectedOptionId = request.AnswerQuestionDTO.selected_option_id;
                }
                else
                {
                    existingAnswer.Answers.Add(new Answer
                    {
                        QuestionId = request.AnswerQuestionDTO.question_id,
                        SelectedOptionId = request.AnswerQuestionDTO.selected_option_id
                    });
                }

                attemptRepo.Update(existingAnswer);
            }
            else
            {
                var attem = new Attempt
                {
                    Id = request.AnswerQuestionDTO.AttemptId,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            QuestionId = request.AnswerQuestionDTO.question_id,
                            SelectedOptionId = request.AnswerQuestionDTO.selected_option_id
                        }
                    }
                };

                 attemptRepo.Add(attem);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result<AnsewrQuestionResponse>.Success(new AnsewrQuestionResponse
            {
                Saved = true
            });
        }

    }
    }

