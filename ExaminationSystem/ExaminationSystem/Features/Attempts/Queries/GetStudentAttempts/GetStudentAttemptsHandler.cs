using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using MailKit.Search;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    public class GetStudentAttemptsHandler(IGenericRepository<Attempt> _genericRepo)
               : IRequestHandler<GetStudentAttemptsQuery,
                                 Result<PaginatedResult<AttemptDto>>>
    {
        public async Task<Result<PaginatedResult<AttemptDto>>> Handle(
            GetStudentAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _genericRepo.AsQueryable().AsNoTracking();

            query = ApplyFiltering(query, request);

            query = ApplySorting(query, request.SortBy, request.Order);

            var totalCount = await query.CountAsync(cancellationToken);

            // Pagination and Projection
            var items = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(a => new AttemptDto
                {
                    AttemptId = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.UserName,
                    QuizTitle = a.Quiz.Title,
                    Score = a.Score,
                    Status = a.Score >= a.Quiz.PassScore ? "Passed" : "Failed",
                    SubmittedAt = a.SubmittedAt ?? a.CreatedAt,
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<AttemptDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PerPage = request.PerPage
            };

            return Result<PaginatedResult<AttemptDto>>.Success(result);
        }

        private IQueryable<Attempt> ApplyFiltering(
            IQueryable<Attempt> query,
            GetStudentAttemptsQuery request)
        {
            // To optimize for the composite index on (QuizId, StudentId)
            if (request.QuizId.HasValue && request.StudentId.HasValue)
                return query.Where(a =>
                    a.QuizId == request.QuizId.Value &&
                    a.StudentId == request.StudentId.Value);

            // QuizId Only 
            if (request.QuizId.HasValue)
                query = query.Where(a => a.QuizId == request.QuizId.Value);

            // StudentId Only
            if (request.StudentId.HasValue)
                query = query.Where(a => a.StudentId == request.StudentId.Value);

            return query;
        }

        private IQueryable<Attempt> ApplySorting(IQueryable<Attempt> query,
                                                 string? sortBy,
                                                 string? order)
        {
            sortBy = sortBy?.ToLower() ?? "submitted_at";
            order = order?.ToLower() ?? "desc";

            if (sortBy == "score")
            {
                return order == "asc"
                    ? query.OrderBy(a => a.Score)
                    : query.OrderByDescending(a => a.Score);
            }

            // default: submitted_at
            return order == "asc"
                ? query.OrderBy(a => a.SubmittedAt)
                : query.OrderByDescending(a => a.SubmittedAt);
        }
    }
}
