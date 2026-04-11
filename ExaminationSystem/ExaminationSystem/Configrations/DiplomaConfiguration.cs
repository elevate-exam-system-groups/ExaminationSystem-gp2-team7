using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
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

            // Soft Delete
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

            // Seed Data
            var adminId = Guid.Parse("A1111111-1111-1111-1111-111111111111");

            builder.HasData(
                new Diploma
                {
                    Id = Guid.Parse("D1111111-1111-1111-1111-111111111111"),
                    AdminId = adminId,
                    Title = "Full-Stack Web Development",
                    Description = "Master front-end and back-end technologies including HTML, CSS, JavaScript, C#, and ASP.NET Core.",
                    Status = DiplomaStatus.Published,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Diploma
                {
                    Id = Guid.Parse("D2222222-2222-2222-2222-222222222222"),
                    AdminId = adminId,
                    Title = "Data Science & Machine Learning",
                    Description = "Learn Python, statistics, data analysis, and ML algorithms from scratch to advanced level.",
                    Status = DiplomaStatus.Published,
                    CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Diploma
                {
                    Id = Guid.Parse("D3333333-3333-3333-3333-333333333333"),
                    AdminId = adminId,
                    Title = "Mobile App Development",
                    Description = "Build cross-platform mobile applications using Flutter and Dart.",
                    Status = DiplomaStatus.Draft,
                    CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
