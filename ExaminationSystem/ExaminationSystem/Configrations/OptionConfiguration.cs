using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.ToTable("Options");

            builder.Property(o => o.MCQQuestionId)
                .IsRequired();

            builder.Property(o => o.OptionText)
                .IsRequired();

            builder.Property(o => o.IsCorrect)
                .IsRequired();

            builder.Property(o => o.OrderIndex)
                .IsRequired();

            // Soft Delete
            builder.HasQueryFilter(o => o.DeletedAt == null);

            builder.HasOne(o => o.MCQQuestion)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.MCQQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.SelectedAnswers)
                .WithOne(a => a.SelectedOption)
                .HasForeignKey(a => a.SelectedOptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
