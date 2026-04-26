using ExaminationSystem.Configurations;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ExaminationSystem.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<StudentDiploma> StudentDiplomas { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<TrueFalseQuestion> TrueFalseQuestions { get; set; }
        public DbSet<QuestionOption> Options { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Answer> Answers { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure owned entity: RefreshToken belongs to ApplicationUser
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasQueryFilter(rt => rt.User.DeletedAt == null);

            // Composite Index
            modelBuilder.Entity<Attempt>()
                        .HasIndex(a => new { a.QuizId, a.StudentId });

            modelBuilder.Entity<ApplicationUser>().Ignore(u => u.IsDeleted);
            modelBuilder.Entity<Diploma>().Ignore(d => d.IsDeleted);
            modelBuilder.Entity<StudentDiploma>().Ignore(sd => sd.IsDeleted);
            modelBuilder.Entity<Quiz>().Ignore(q => q.IsDeleted);
            modelBuilder.Entity<Question>().Ignore(q => q.IsDeleted);
            modelBuilder.Entity<QuestionOption>().Ignore(o => o.IsDeleted);
            modelBuilder.Entity<Attempt>().Ignore(a => a.IsDeleted);
            modelBuilder.Entity<Answer>().Ignore(a => a.IsDeleted);

            
            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new DiplomaConfiguration());
            modelBuilder.ApplyConfiguration(new StudentDiplomaConfiguration());
            modelBuilder.ApplyConfiguration(new QuizConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new OptionConfiguration());
            modelBuilder.ApplyConfiguration(new AttemptConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        }
    }
}
