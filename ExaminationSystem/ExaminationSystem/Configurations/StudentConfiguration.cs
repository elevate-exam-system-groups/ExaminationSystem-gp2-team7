using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Email)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasDefaultValue(StudentStatus.Pending);

           
            builder.Property(s => s.EmailOtpCode)
                .HasMaxLength(100);  

            builder.Property(s => s.EmailOtpAttempts)
                .HasDefaultValue(0);

           
            builder.Property(s => s.ResetToken)
                .HasMaxLength(500);

            builder.HasMany(s => s.Enrollments)
                .WithOne(se => se.Student)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Attempts)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
