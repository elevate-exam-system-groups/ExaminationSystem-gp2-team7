using System.Security.Claims;
using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard
{
    public class GetStudentDashboardHandler(ApplicationDbContext _context,
                                            IMemoryCache _cache,
                                            IHttpContextAccessor _httpContextAccessor)
        : IRequestHandler<GetStudentDashboardQuery, Result<StudentDashboardDto>>
    {
        public async Task<Result<StudentDashboardDto>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
        {
            var studentIdValue = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(studentIdValue, out var studentId))
                return Result<StudentDashboardDto>.Failure("Invalid student id", 400);

            var cacheKey = $"student-dashboard-{studentId}";

            if (_cache.TryGetValue(cacheKey, out StudentDashboardDto cached))
                return Result<StudentDashboardDto>.Success(cached);

            var data = await GetStudentDashboardData(studentId, cancellationToken);

            var dto = new StudentDashboardDto
            {
                EnrolledDiplomas = data.EnrolledDiplomas,
                RecentQuizAttempts = data.RecentQuizAttempts,
                OverallStats = new DashboardStatsDto
                {
                    TotalQuizzesTaken = data.OverallStats.TotalQuizzesTaken,
                    AvgScore = data.OverallStats.AvgScore,
                    PassRate = data.OverallStats.PassRate
                }
            };

            _cache.Set(cacheKey, dto, TimeSpan.FromSeconds(60));

            return Result<StudentDashboardDto>.Success(dto);
        }

        private async Task<StudentDashboardDto> GetStudentDashboardData(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            // Enrolled Diplomas
            var diplomas = await _context.StudentDiplomas
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => new EnrolledDiplomaDto
                {
                    DiplomaId = e.Diploma.Id,
                    Title = e.Diploma.Title,
                    Progress = e.Progress
                })
                .ToListAsync(cancellationToken);

            //Recent Quiz Attempts
            var attempts = await _context.Attempts
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .Select(a => new QuizAttemptDto
                {
                    QuizId = a.QuizId,
                    Score = a.Score,
                    Date = a.CreatedAt
                })
                .ToListAsync(cancellationToken);

            // Overall Stats
            var statsList = await _context.Attempts
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .Select(a => new
                {
                    a.Score,
                    a.Quiz.PassScore
                })
                .ToListAsync(cancellationToken);

            var total = statsList.Count;

            var stats = new
            {
                Total = total,
                Avg = total > 0 ? statsList.Average(q => q.Score) : 0,
                PassRate = total > 0
                    ? statsList.Count(q => q.Score >= q.PassScore) * 100.0 / total
                    : 0
            };

            return new StudentDashboardDto
            {
                EnrolledDiplomas = diplomas,
                RecentQuizAttempts = attempts,
                OverallStats = new DashboardStatsDto
                {
                    TotalQuizzesTaken = stats?.Total ?? 0,
                    AvgScore = stats?.Avg ?? 0,
                    PassRate = stats?.PassRate ?? 0
                }
            };
    }
    }
}
