using ExaminationSystem.Models;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public static class DiplomaFilters
    {
        public static IQueryable<Diploma> FilterByTitle(this IQueryable<Diploma> query, string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return query;

            return query.Where(d => d.Title.Contains(title));
        }
        public static IQueryable<Diploma> FilterByStudent(this IQueryable<Diploma> query, Guid? studentId)
        {
            if (!studentId.HasValue)
                return query;

            return query.Where(d => d.StudentEnrollments.Any(e => e.StudentId == studentId));
        }
    }
}
