using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetQuizHistory
{
    public class GetQuizHistoryHandler
        : IRequestHandler<GetQuizHistoryQuery, Result<PagedResponse<QuizHistoryItemResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizHistoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedResponse<QuizHistoryItemResponse>>> Handle(
            GetQuizHistoryQuery request, CancellationToken cancellationToken)
        {
           
            var query = _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .Where(a => a.StudentId == request.StudentId);

           
            if (request.QuizId.HasValue)
                query = query.Where(a => a.QuizId == request.QuizId.Value);

            
            if (request.DiplomaId.HasValue)
                query = query.Where(a => a.Quiz.DiplomaId == request.DiplomaId.Value);

            
            var total = await query.CountAsync(cancellationToken);

            
            var items = await query
                .OrderByDescending(a => a.SubmittedAt)
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(a => new QuizHistoryItemResponse
                {
                    AttemptId = a.Id,
                    QuizTitle = a.Quiz.Title,
                    Score = a.Score,
                    Passed = a.Passed,
                    Status = a.Status.ToString(),
                    SubmittedAt = a.SubmittedAt
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var pagedResult = new PagedResponse<QuizHistoryItemResponse>
            {
                Data = items,
                Page = request.Page,
                PageSize = request.PerPage,
                Total = total
            };

            return Result<PagedResponse<QuizHistoryItemResponse>>.Success(pagedResult);
        }
    }
}
