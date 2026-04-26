namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
