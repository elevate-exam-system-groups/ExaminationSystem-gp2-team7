using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Commands.AddDiploma
{
    public record AddDiplomaCommand(string Title, 
                                    string? Description,
                                    int TotalQuizCount) 
                 : IRequest<Result<string>>;
}
