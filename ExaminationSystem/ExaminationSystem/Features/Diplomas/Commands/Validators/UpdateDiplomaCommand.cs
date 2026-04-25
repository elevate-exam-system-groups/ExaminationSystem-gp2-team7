namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public class UpdateDiplomaCommand : IDiplomaCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TotalQuizCount { get; set; }
    }
}
