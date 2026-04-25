namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public class AddDiplomaCommand : IDiplomaCommand
    {
        public string Title { get; set; }
        public int TotalQuizCount { get; set; }
    }
}
