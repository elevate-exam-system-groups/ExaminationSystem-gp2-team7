using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.AdminDashboardStats
{
    public class GetAdminStatusHandler : IRequestHandler<GetAdminStatsQuery, Result<AdminStatsResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;


        public GetAdminStatusHandler(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<AdminStatsResponse>> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
        {
            // 1. Check Role
            if (request.UserRole != "Admin")
                return Result<AdminStatsResponse>.Failure("Forbidden", StatusCodes.Status403Forbidden);

            // Users (Identity)
            var totalUsers = userManager.Users.Count();

            // 2. Data
            var quizzesRepo = unitOfWork.GetRepository<Quiz>();
            var attemptsRepo = unitOfWork.GetRepository<Attempt>();

            var totalQuizzes = (await quizzesRepo.GetAllAsync()).Count();
            var totalAttempts = (await attemptsRepo.GetAllAsync()).Count();

            var today = DateTime.UtcNow.Date;

            var activeUsersToday = userManager.Users
                .Count(u => u.LastLoginDate.HasValue &&
                            u.LastLoginDate.Value.Date == today);

            var attempts = await attemptsRepo.GetAllAsync();


            var passedAttempts = attempts.Count(a => a.Status == AttemptStatus.IsPassed);
            var avgPassRate = totalAttempts == 0 ? 0: (double)passedAttempts / totalAttempts * 100;

            var response = new AdminStatsResponse
            {
                TotalUsers = totalUsers,
                ActiveUsersToday = activeUsersToday,
                TotalQuizzes = totalQuizzes,
                TotalAttempts = totalAttempts,
                AvgPassRate = avgPassRate
            };

            return Result<AdminStatsResponse>.Success(response);

        }
    }
}
