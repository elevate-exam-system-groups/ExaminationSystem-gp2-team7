namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public interface IDiplomaCommand
    {
        string Title { get; }
        int TotalQuizCount { get; }
    }
}
