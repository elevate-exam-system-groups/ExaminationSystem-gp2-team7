using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Configurations
{
    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.ToTable("Diplomas");

            builder.Property(d => d.AdminId)
                .IsRequired();

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .HasMaxLength(1000);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasDefaultValue(DiplomaStatus.Draft);

           
            builder.HasQueryFilter(d => d.DeletedAt == null);

            builder.HasOne(d => d.Admin)
                .WithMany(a => a.CreatedDiplomas)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Quizzes)
                .WithOne(q => q.Diploma)
                .HasForeignKey(q => q.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.StudentEnrollments)
                .WithOne(se => se.Diploma)
                .HasForeignKey(se => se.DiplomaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
