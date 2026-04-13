using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quizzes");

            builder.Property(q => q.DiplomaId)
                .IsRequired();

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(q => q.DurationMinutes)
                .IsRequired();

            builder.Property(q => q.PassScore)
                .IsRequired()
                .HasPrecision(5, 2)
                .HasDefaultValue(60m);

            builder.Property(q => q.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(QuizStatus.Draft);

            
            builder.HasQueryFilter(q => q.DeletedAt == null);

            builder.HasOne(q => q.Diploma)
                .WithMany(d => d.Quizzes)
                .HasForeignKey(q => q.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Questions)
                .WithOne(qu => qu.Quiz)
                .HasForeignKey(qu => qu.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Attempts)
                .WithOne(a => a.Quiz)
                .HasForeignKey(a => a.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
