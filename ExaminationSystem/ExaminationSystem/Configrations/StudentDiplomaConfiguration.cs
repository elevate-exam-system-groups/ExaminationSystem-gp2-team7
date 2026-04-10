using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
{
    public class StudentDiplomaConfiguration : IEntityTypeConfiguration<StudentDiploma>
    {
        public void Configure(EntityTypeBuilder<StudentDiploma> builder)
        {
            builder.ToTable("StudentDiplomas");

            builder.HasKey(se => se.EnrollmentId);

            builder.Property(se => se.StudentId)
                .IsRequired();

            builder.Property(se => se.DiplomaId)
                .IsRequired();

            builder.Property(se => se.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(EnrollmentStatus.Active);

            builder.Property(se => se.Progress)
                .HasPrecision(5, 2)
                .HasDefaultValue(0m);

            // Soft Delete
            builder.HasQueryFilter(se => se.DeletedAt == null);

            builder.HasOne(se => se.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(se => se.Diploma)
                .WithMany(d => d.StudentEnrollments)
                .HasForeignKey(se => se.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
