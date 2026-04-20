using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries.GetDiplomaQuizzes
{
    public record GetDiplomaQuizzesQuery(Guid DiplomaId, Guid StudentId) : IRequest<Result<List<DiplomaQuizResponse>>>;
}
