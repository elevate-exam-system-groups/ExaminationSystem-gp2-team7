using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
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

            builder.Property(s => s.UserType)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasDefaultValue(StudentStatus.Pending);

            // OTP
            builder.Property(s => s.EmailOtpCode)
                .HasMaxLength(100);  // Hashed OTP

            builder.Property(s => s.EmailOtpAttempts)
                .HasDefaultValue(0);

            // Reset Token
            builder.Property(s => s.ResetToken)
                .HasMaxLength(500);  // JWT Token

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
