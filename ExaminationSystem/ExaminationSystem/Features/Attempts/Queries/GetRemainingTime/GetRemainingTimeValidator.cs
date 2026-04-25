using ExaminationSystem.Common;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Attempts.Queries.GetRemainingTime
{
    public static class GetRemainingTimeValidator
    {
        public static Result<RemainingTimeResponse>? Validate(
           GetRemainingTimeQuery request, 
           Guid studentId, 
           AttemptStatus attemptStatus)
        {


            // Validate authorization
            if (!request.IsAdmin && studentId != request.UserId)
            {
                return Result<RemainingTimeResponse>.Failure(
                    "You are not authorized to view this attempt's timer.",
                    StatusCodes.Status403Forbidden);
            }

            // Validate status
            if (attemptStatus != AttemptStatus.InProgress)
            {
                return Result<RemainingTimeResponse>.Success(
                    new RemainingTimeResponse(0));
            }

            // Validation Passed
            return null;
        }
    }
}
