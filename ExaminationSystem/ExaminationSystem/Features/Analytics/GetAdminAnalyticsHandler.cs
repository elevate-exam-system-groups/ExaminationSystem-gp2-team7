using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static ExaminationSystem.Features.Analytics.Dtos;

namespace ExaminationSystem.Features.Analytics
{
    public class GetAdminAnalyticsHandler(IGenericRepository<Attempt> _genericRepo,
                                          IMemoryCache _cache)
        : IRequestHandler<GetAdminAnalyticsQuery, Result<AdminAnalyticsResponse>>
    {
        public async Task<Result<AdminAnalyticsResponse>> Handle(GetAdminAnalyticsQuery request, CancellationToken ct)
        {
            var cacheKey = BuildCacheKey(request);

            if (_cache.TryGetValue(cacheKey, out AdminAnalyticsResponse cached))
                return Result<AdminAnalyticsResponse>.Success(cached);

            var attempts = BuildAttemptsQuery(request);

            var passRateByQuiz = await GetPassRateByQuiz(attempts, ct);
            var avgScoreByDiploma = await GetAvgScoreByDiploma(attempts, ct);
            var attemptsOverTime = await GetAttemptsOverTime(attempts, ct);
            //var topFailedQuestions = await GetTopFailedQuestions(attempts, ct);

            var result = new AdminAnalyticsResponse
            {
                PassRateByQuiz = passRateByQuiz,
                AvgScoreByDiploma = avgScoreByDiploma,
                AttemptsOverTime = attemptsOverTime,
                //TopFailedQuestions = topFailedQuestions
            };

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            return Result<AdminAnalyticsResponse>.Success(result);
        }

        private IQueryable<Attempt> BuildAttemptsQuery(GetAdminAnalyticsQuery request)
        {
            var query = _genericRepo.AsQueryable();

            if (request.From.HasValue)
                query = query.Where(a => a.CreatedAt >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(a => a.CreatedAt <= request.To.Value);

            if (request.DiplomaId.HasValue)
                query = query.Where(a => a.Quiz.DiplomaId == request.DiplomaId);

            return query;
        }

        private async Task<List<PassRateByQuizDto>> GetPassRateByQuiz(IQueryable<Attempt> attempts, CancellationToken ct)
        {
            return await attempts
                .GroupBy(a => new { a.QuizId, a.Quiz.Title })
                .Select(g => new PassRateByQuizDto(
                    g.Key.QuizId,
                    g.Key.Title,
                    g.Count(x => x.Score >= x.Quiz.PassScore) * 1.0 / g.Count()
                ))
                .ToListAsync(ct);
        }

        private async Task<List<AvgScoreByDiplomaDto>> GetAvgScoreByDiploma(IQueryable<Attempt> attempts, CancellationToken ct)
        {
            return await attempts
                .GroupBy(a => new { a.Quiz.DiplomaId, a.Quiz.Diploma.Title })
                .Select(g => new AvgScoreByDiplomaDto(
                    g.Key.DiplomaId,
                    g.Key.Title,
                    g.Average(x => x.Score)
                ))
                .ToListAsync(ct);
        }

        private async Task<List<AttemptsOverTimeDto>> GetAttemptsOverTime(IQueryable<Attempt> attempts, CancellationToken ct)
        {
            return await attempts
                .GroupBy(a => a.CreatedAt.Date)
                .Select(g => new AttemptsOverTimeDto(
                    g.Key,
                    g.Count()
                ))
                .ToListAsync(ct);
        }

        private string BuildCacheKey(GetAdminAnalyticsQuery request)
        {
            return $"analytics_{request.From}_{request.To}_{request.DiplomaId}";
        }
    }
}
