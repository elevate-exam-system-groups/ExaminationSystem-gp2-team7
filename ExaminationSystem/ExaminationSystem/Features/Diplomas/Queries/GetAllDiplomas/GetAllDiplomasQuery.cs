using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public record GetAllDiplomasQuery(string? Title, 
                                      int Page, 
                                      int PageSize) 
        : /*IRequest<IEnumerable<DiplomaResponse>>;*/
          IRequest<PagedResponse<DiplomaResponse>>;

}