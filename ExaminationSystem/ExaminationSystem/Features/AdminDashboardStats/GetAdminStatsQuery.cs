using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using MediatR;

namespace ExaminationSystem.Features.AdminDashboardStats
{

    public class GetAdminStatsQuery() : IRequest<Result<AdminStatsResponse>>
    {
        public string UserRole { get; set; } = default!;

    }

}
