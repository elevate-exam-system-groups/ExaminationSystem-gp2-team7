using System.Security.Claims;
using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Repositories;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas;

public class GetAllDiplomasHandler(IGenericRepository<Diploma> _genericRepo,
                                   IMemoryCache cache,
                                   IHttpContextAccessor httpContextAccessor) 
              : IRequestHandler<GetAllDiplomasQuery, PagedResponse<DiplomaResponse>>
{
    public async Task<PagedResponse<DiplomaResponse>> Handle(
        GetAllDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 6 : request.PageSize;

        var studentId = GetStudentId();

        var cacheKey = BuildCacheKey(studentId, request, page, pageSize);

        if (cache.TryGetValue(cacheKey, out PagedResponse<DiplomaResponse> cached))
            return cached;

        var query = _genericRepo.AsQueryable()
                                .Where(d => d.Status == DiplomaStatus.Published)
                                .FilterByTitle(request.Title);

        if (request.OnlyEnrolled)
            query = query.FilterByStudent(studentId);

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .OrderBy(x => x.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new DiplomaResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                TotalQuizCount = x.Quizzes.Count,

                Progress = x.StudentEnrollments
                    .Where(e => e.StudentId == studentId)
                    .Select(e => e.Progress)
                    .FirstOrDefault()
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = new PagedResponse<DiplomaResponse>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            Total = total
        };

        cache.Set(cacheKey, result, GetCacheOptions());

        return result;
    }

    private Guid? GetStudentId()
    {
        var id = httpContextAccessor.HttpContext?
            .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(id, out var userId))
            return userId;

        return null;
    }

    private string BuildCacheKey(Guid? studentId, GetAllDiplomasQuery request, int page, int pageSize)
    {
        return $"diplomas_{studentId?.ToString() ?? "anon"}_{request.Title?.Trim().ToLower() ?? "all"}_{page}_{pageSize}";
    }

    private static MemoryCacheEntryOptions GetCacheOptions()
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
    }
}