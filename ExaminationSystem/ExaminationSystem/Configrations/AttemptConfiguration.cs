using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.ToTable("Attempts");

            builder.Property(a => a.StudentId)
                .IsRequired();

            builder.Property(a => a.QuizId)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(AttemptStatus.InProgress);

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.Score)
                .HasPrecision(5, 2);

            // Soft Delete
            builder.HasQueryFilter(a => a.DeletedAt == null);

            builder.HasOne(a => a.Student)
                .WithMany(s => s.Attempts)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Quiz)
                .WithMany(q => q.Attempts)
                .HasForeignKey(a => a.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Answers)
                .WithOne(an => an.Attempt)
                .HasForeignKey(an => an.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
