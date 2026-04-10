using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasDiscriminator<string>("QuestionType")
                .HasValue<MultipleChoiceQuestion>("MCQ")
                .HasValue<TrueFalseQuestion>("TrueFalse");

            builder.Property(q => q.QuizId)
                .IsRequired();

            builder.Property(q => q.QuestionText)
                .IsRequired();

            builder.Property(q => q.OrderIndex)
                .IsRequired();

            builder.Property(q => q.QuestionType)
                .IsRequired()
                .HasMaxLength(50);

            // Soft Delete
            builder.HasQueryFilter(q => q.DeletedAt == null);

            builder.HasOne(q => q.Quiz)
                .WithMany(qu => qu.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
