using ExaminationSystem.Common;
using ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos;
using MediatR;

namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard
{
    public record GetStudentDashboardQuery : IRequest<Result<StudentDashboardDto>>;
}
