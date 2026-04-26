using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Analytics
{
    public record GetAdminAnalyticsQuery(DateTime? From,
                                         DateTime? To,
                                         Guid? DiplomaId) 
         : IRequest<Result<AdminAnalyticsResponse>>;
}
