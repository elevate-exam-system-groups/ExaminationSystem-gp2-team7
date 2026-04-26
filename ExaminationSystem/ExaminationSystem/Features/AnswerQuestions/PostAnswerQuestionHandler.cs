//using E_Commerce.Domain.Contracts;
//using ExaminationSystem.Common;
//using ExaminationSystem.Models;
//using MediatR;

//namespace ExaminationSystem.Features.AnswerQuestions
//{
//    public class PostAnswerQuestionHandler : IRequestHandler<PostAswerQuestionCommand, Result<AnsewrQuestionResponse>>
//    {
//        private readonly IUnitOfWork unitOfWork;

//        public PostAnswerQuestionHandler(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;
//        }   
//        public async Task<Result<AnsewrQuestionResponse>> Handle(PostAswerQuestionCommand request, CancellationToken cancellationToken)
//        {
//            // 1. Get Attempt

//            var attemp = unitOfWork.GetRepository<Question>().GetByIdAsync(request.AnswerQuestionDTO.question_id);

//            if (attemp is null)
//            {
//                return Task.FromResult(Result<AnsewrQuestionResponse>.Failure("Attempt not found", StatusCodes.Status404NotFound));
//            }

//            // 2. Check ownership
//            //if (attemp.StudentId = request.AnswerQuestionDTO.question_id)
//            //{

//            //}



//        }
//    }
//}
