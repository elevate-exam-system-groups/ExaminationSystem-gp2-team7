using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using MediatR;

namespace ExaminationSystem.Features.AnswerQuestions
{
    
    public record PostAswerQuestionCommand(AnswerQuestionDTO AnswerQuestionDTO) : IRequest<Result<AnsewrQuestionResponse>>;

}
