using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;

namespace ExaminationSystem.Features.Diplomas.Queries.GetDiplomaQuizzes
{
    public class GetDiplomaQuizzesHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache) 
        : IRequestHandler<GetDiplomaQuizzesQuery, Result<List<DiplomaQuizResponse>>>
    {
        private static readonly MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheKeys.Durations.Short);

        public async Task<Result<List<DiplomaQuizResponse>>> Handle(
            GetDiplomaQuizzesQuery request, CancellationToken cancellationToken)
        {
            // Define a unique cache key based on both DiplomaId and StudentId
            var cacheKey = CacheKeys.GetDiplomaQuizzesCacheKey(request.DiplomaId, request.StudentId);

            // Check if the result is already in the cache
            if (memoryCache.TryGetValue(cacheKey, out List<DiplomaQuizResponse>? cached))
            {
                return Result<List<DiplomaQuizResponse>>.Success(cached!);
            }

            // Validate diploma exists and is Published
            var diploma = await GetDiplomaStatusAsync(request.DiplomaId, cancellationToken);

            if (!diploma.Exists) // If record doesn't exist
                return Result<List<DiplomaQuizResponse>>.Failure(
                    "Diploma not found.", StatusCodes.Status404NotFound);

            if (diploma.Status != Models.Enums.DiplomaStatus.Published)
                return Result<List<DiplomaQuizResponse>>.Failure(
                    "Diploma is not currently accessible.", StatusCodes.Status403Forbidden);



            // Query published quizzes with student attempt stats
            var quizzes = await FetchDiplomaQuizzesAsync(request, cancellationToken);

            memoryCache.Set(cacheKey, quizzes, CacheOptions);

            return Result<List<DiplomaQuizResponse>>.Success(quizzes);
        }

      

        // ── Helpers ────────────────────────────────────────────────────────────────

        private async Task<(bool Exists, DiplomaStatus Status)> GetDiplomaStatusAsync(
            Guid diplomaId, CancellationToken cancellationToken)
        {
            return await unitOfWork.GetRepository<Diploma>().AsQueryable()
                .Where(d => d.Id == diplomaId)
                .Select(d => ValueTuple.Create(true, d.Status))
                .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<List<DiplomaQuizResponse>> FetchDiplomaQuizzesAsync(
            GetDiplomaQuizzesQuery request, CancellationToken cancellationToken)
        {
            var quizzes = await unitOfWork.GetRepository<Quiz>().AsQueryable()
                            .Where(q => q.DiplomaId == request.DiplomaId
                                && q.Status == QuizStatus.Published)
                            .Select(q => new
                            {
                                q.Id,
                                q.Title,
                                q.DurationMinutes,
                                AttemptCount = q.Attempts.Count(a => a.StudentId == request.StudentId),
                                LatestAttempt = q.Attempts
                                    .Where(a => a.StudentId == request.StudentId)
                                    .OrderByDescending(a => a.StartTime)
                                    .Select(a => new { a.Status, a.Score })
                                    .FirstOrDefault()
                            })
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);

            // Map in memory
            var response = quizzes.ConvertAll(
                    q => new DiplomaQuizResponse
                    {
                        Id = q.Id,
                        Title = q.Title,
                        DurationMinutes = q.DurationMinutes,
                        AttemptCount = q.AttemptCount,
                        LastScore = q.LatestAttempt?.Score ?? 0,
                        Status = q.LatestAttempt?.Status
                    }
                );

            return response;
        }
    }
}
