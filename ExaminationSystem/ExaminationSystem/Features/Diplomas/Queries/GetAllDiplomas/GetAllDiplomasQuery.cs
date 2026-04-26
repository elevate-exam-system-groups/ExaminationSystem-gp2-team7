using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public record GetAllDiplomasQuery(string? Title,
                                      bool OnlyEnrolled,
                                      int Page,
                                      int PageSize)
        : /*IRequest<IEnumerable<DiplomaResponse>>;*/
          IRequest<Result<PagedResponse<DiplomaResponse>>>;

}