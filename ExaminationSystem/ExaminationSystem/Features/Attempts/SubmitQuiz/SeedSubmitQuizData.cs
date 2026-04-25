using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    /// <summary>
    /// Seed data للتيست بتاع Submit Quiz
    /// بيعمل: Student + Diploma + Quiz + 5 أسئلة + محاولة InProgress + 5 إجابات (3 صح + 2 غلط)
    /// </summary>
    public static class SeedSubmitQuizData
    {
        // IDs ثابتة عشان نقدر نستخدمها في التيست
        public static readonly Guid StudentId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0001");
        public static readonly Guid DiplomaId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0002");
        public static readonly Guid QuizId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0003");
        public static readonly Guid AttemptId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0004");
        public static readonly Guid AdminId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeee0005");

        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            // لو البيانات موجودة بالفعل → متعملش حاجة
            if (await context.Attempts.AnyAsync(a => a.Id == AttemptId))
                return;

            // ============================
            // 1️⃣ اعمل Student
            // ============================
            var student = await userManager.FindByEmailAsync("student.test@mail.com");
            if (student == null)
            {
                student = new Student
                {
                    Id = StudentId,
                    FullName = "Test Student",
                    Email = "student.test@mail.com",
                    UserName = "student.test@mail.com",
                    EmailConfirmed = true,
                    Status = StudentStatus.Active
                };

                var result = await userManager.CreateAsync(student, "Student@123");
                if (!result.Succeeded)
                    throw new Exception("Failed to create test student: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(student, "Student");
            }

            // ============================
            // 2️⃣ تأكد إن فيه Admin في جدول Admins
            // ============================
            var admin = await context.Admins.FirstOrDefaultAsync();
            if (admin == null)
            {
                // مفيش Admin → نعمل واحد للتيست
                var testAdmin = new Admin
                {
                    Id = AdminId,
                    FullName = "Test Admin",
                    Email = "testadmin@mail.com",
                    UserName = "testadmin@mail.com",
                    EmailConfirmed = true,
                    Status = AdminStatus.Active
                };

                var adminResult = await userManager.CreateAsync(testAdmin, "Admin@123");
                if (!adminResult.Succeeded)
                    throw new Exception("Failed to create test admin: " +
                        string.Join(", ", adminResult.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(testAdmin, "Admin");
                admin = testAdmin;
            }

            if (!await context.Diplomas.AnyAsync(d => d.Id == DiplomaId))
            {
                var diploma = new Diploma
                {
                    Id = DiplomaId,
                    Title = "Test Diploma - C# Fundamentals",
                    Description = "دبلومة تجريبية للتيست",
                    TotalQuizCount = 1,
                    Status = DiplomaStatus.Published,
                    AdminId = admin.Id  // ← نستخدم الأدمن الحقيقي من الداتابيز
                };
                context.Diplomas.Add(diploma);
                await context.SaveChangesAsync();
            }

            // ============================
            // 3️⃣ اعمل Quiz
            // ============================
            if (!await context.Quizzes.AnyAsync(q => q.Id == QuizId))
            {
                var quiz = new Quiz
                {
                    Id = QuizId,
                    Title = "C# Basics Quiz",
                    DurationMinutes = 30,
                    PassScore = 60,
                    MaxAttempts = 3,
                    Instructions = "اختار الإجابة الصحيحة",
                    Status = QuizStatus.Published,
                    TotalQuestionsCache = 5,
                    DiplomaId = DiplomaId
                };
                context.Quizzes.Add(quiz);
                await context.SaveChangesAsync(); // احفظ الـ Quiz قبل الأسئلة
            }

            // ============================
            // 4️⃣ اعمل 5 أسئلة True/False
            // ============================
            var questions = new List<TrueFalseQuestion>
            {
                new TrueFalseQuestion
                {
                    QuestionText = "C# is a compiled language",
                    CorrectAnswer = true,
                    OrderIndex = 1,
                    QuestionType = "TrueFalse",
                    QuizId = QuizId
                },
                new TrueFalseQuestion
                {
                    QuestionText = "string is a value type in C#",
                    CorrectAnswer = false,
                    OrderIndex = 2,
                    QuestionType = "TrueFalse",
                    QuizId = QuizId
                },
                new TrueFalseQuestion
                {
                    QuestionText = "int is 32-bit in C#",
                    CorrectAnswer = true,
                    OrderIndex = 3,
                    QuestionType = "TrueFalse",
                    QuizId = QuizId
                },
                new TrueFalseQuestion
                {
                    QuestionText = "C# supports multiple inheritance for classes",
                    CorrectAnswer = false,
                    OrderIndex = 4,
                    QuestionType = "TrueFalse",
                    QuizId = QuizId
                },
                new TrueFalseQuestion
                {
                    QuestionText = "async methods must return void",
                    CorrectAnswer = false,
                    OrderIndex = 5,
                    QuestionType = "TrueFalse",
                    QuizId = QuizId
                }
            };
            context.TrueFalseQuestions.AddRange(questions);
            await context.SaveChangesAsync(); // احفظ الأسئلة عشان ناخد الـ IDs

            // ============================
            // 5️⃣ اعمل Attempt (InProgress)
            // ============================
            var attempt = new Attempt
            {
                Id = AttemptId,
                StudentId = StudentId,
                QuizId = QuizId,
                Status = AttemptStatus.InProgress,
                StartTime = DateTime.UtcNow.AddMinutes(-10) // بدأ من 10 دقائق (لسه عنده وقت)
            };
            context.Attempts.Add(attempt);

            // ============================
            // 6️⃣ اعمل 5 إجابات (3 صح + 2 غلط)
            // ============================
            var answers = new List<Answer>
            {
                // سؤال 1: "C# is compiled" → الإجابة الصح: true → الطالب جاوب true ✅
                new Answer
                {
                    AttemptId = AttemptId,
                    QuestionId = questions[0].Id,
                    StudentAnswer = true,
                    IsCorrect = true,
                    SubmittedAt = DateTime.UtcNow.AddMinutes(-9)
                },
                // سؤال 2: "string is value type" → الإجابة الصح: false → الطالب جاوب false ✅
                new Answer
                {
                    AttemptId = AttemptId,
                    QuestionId = questions[1].Id,
                    StudentAnswer = false,
                    IsCorrect = true,
                    SubmittedAt = DateTime.UtcNow.AddMinutes(-8)
                },
                // سؤال 3: "int is 32-bit" → الإجابة الصح: true → الطالب جاوب true ✅
                new Answer
                {
                    AttemptId = AttemptId,
                    QuestionId = questions[2].Id,
                    StudentAnswer = true,
                    IsCorrect = true,
                    SubmittedAt = DateTime.UtcNow.AddMinutes(-7)
                },
                // سؤال 4: "multiple inheritance" → الإجابة الصح: false → الطالب جاوب true ❌
                new Answer
                {
                    AttemptId = AttemptId,
                    QuestionId = questions[3].Id,
                    StudentAnswer = true,
                    IsCorrect = false,
                    SubmittedAt = DateTime.UtcNow.AddMinutes(-6)
                },
                // سؤال 5: "async must return void" → الإجابة الصح: false → الطالب جاوب true ❌
                new Answer
                {
                    AttemptId = AttemptId,
                    QuestionId = questions[4].Id,
                    StudentAnswer = true,
                    IsCorrect = false,
                    SubmittedAt = DateTime.UtcNow.AddMinutes(-5)
                }
            };
            context.Answers.AddRange(answers);
            await context.SaveChangesAsync();
        }
    }
}
