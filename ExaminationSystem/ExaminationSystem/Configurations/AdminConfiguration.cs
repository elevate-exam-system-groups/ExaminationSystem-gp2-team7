using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Configurations
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

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasDefaultValue(AdminStatus.Active);

<<<<<<< HEAD:ExaminationSystem/ExaminationSystem/Configurations/AdminConfiguration.cs
           
            builder.Property(a => a.EmailOtpCode)
                .HasMaxLength(100); 
=======

            builder.Property(a => a.EmailOtpCode)
                .HasMaxLength(100);
>>>>>>> dev:ExaminationSystem/ExaminationSystem/Configrations/AdminConfiguration.cs

            builder.Property(a => a.EmailOtpAttempts)
                .HasDefaultValue(0);

<<<<<<< HEAD:ExaminationSystem/ExaminationSystem/Configurations/AdminConfiguration.cs
           
            builder.Property(a => a.ResetToken)
                .HasMaxLength(500); 
=======

            builder.Property(a => a.ResetToken)
                .HasMaxLength(500);
>>>>>>> dev:ExaminationSystem/ExaminationSystem/Configrations/AdminConfiguration.cs

            builder.HasMany(a => a.CreatedDiplomas)
                .WithOne(d => d.Admin)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
