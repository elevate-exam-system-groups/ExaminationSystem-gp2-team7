using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;

namespace ExaminationSystem.Configrations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Email)
                .IsRequired();

            builder.Property(a => a.UserType)
                .IsRequired();

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasDefaultValue(AdminStatus.Active);

            // OTP
            builder.Property(a => a.EmailOtpCode)
                .HasMaxLength(100);  // Hashed OTP

            builder.Property(a => a.EmailOtpAttempts)
                .HasDefaultValue(0);

            // Reset Token
            builder.Property(a => a.ResetToken)
                .HasMaxLength(500);  // JWT Token

            builder.HasMany(a => a.CreatedDiplomas)
                .WithOne(d => d.Admin)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
