using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.Property(a => a.AttemptId)
                .IsRequired();

            builder.Property(a => a.QuestionId)
                .IsRequired();

            builder.Property(a => a.IsCorrect)
                .IsRequired();

            
            builder.HasQueryFilter(a => a.DeletedAt == null);

            builder.HasOne(a => a.Attempt)
                .WithMany(a => a.Answers)
                .HasForeignKey(a => a.AttemptId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.SelectedOption)
                .WithMany(o => o.SelectedAnswers)
                .HasForeignKey(a => a.SelectedOptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
