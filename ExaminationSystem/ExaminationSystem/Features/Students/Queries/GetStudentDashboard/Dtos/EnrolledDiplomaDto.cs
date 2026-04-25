namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos
{
    public class EnrolledDiplomaDto
    {
        public Guid DiplomaId { get; set; }
        public string Title { get; set; }
        public decimal Progress { get; set; }
    }
}
