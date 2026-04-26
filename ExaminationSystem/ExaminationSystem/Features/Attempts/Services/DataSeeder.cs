using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services
{
    public static class DataSeeder
    {
        public static async Task SeedMockDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Skip if data already fully seeded
            if (await context.Answers.AnyAsync())
                return;

            // Get the seeded admin — ensure it exists in the Admins table (TPT)
            var admin = await userManager.FindByEmailAsync("admin@mail.com");
            if (admin == null) return;

            var adminId = admin.Id;

            if (!await context.Admins.AnyAsync(a => a.Id == adminId))
            {
                context.Database.ExecuteSqlInterpolated(
                    $"INSERT INTO Admins (Id, Status) VALUES ({adminId}, {"Active"})");
            }

            // ── Students (idempotent) ──
            var student1 = (Student?)await userManager.FindByEmailAsync("ahmed@mail.com");
            if (student1 == null)
            {
                student1 = new Student
                {
                    Id = Guid.NewGuid(),
                    FullName = "Ahmed Hassan",
                    Email = "ahmed@mail.com",
                    UserName = "ahmed@mail.com",
                    Status = StudentStatus.Active,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student1, "Student@123");
                await userManager.AddToRoleAsync(student1, "Student");
            }

            var student2 = (Student?)await userManager.FindByEmailAsync("sara@mail.com");
            if (student2 == null)
            {
                student2 = new Student
                {
                    Id = Guid.NewGuid(),
                    FullName = "Sara Mohamed",
                    Email = "sara@mail.com",
                    UserName = "sara@mail.com",
                    Status = StudentStatus.Active,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student2, "Student@123");
                await userManager.AddToRoleAsync(student2, "Student");
            }

            var student1Id = student1.Id;
            var student2Id = student2.Id;

            // ── Diplomas (idempotent) ──
            var diploma1 = await context.Diplomas.FirstOrDefaultAsync(d => d.Title == "Full-Stack Web Development");
            if (diploma1 == null)
            {
                diploma1 = new Diploma
                {
                    Id = Guid.NewGuid(),
                    Title = "Full-Stack Web Development",
                    Description = "A comprehensive diploma covering front-end and back-end web technologies.",
                    TotalQuizCount = 2,
                    Status = DiplomaStatus.Published,
                    AdminId = adminId,
                    CreatedBy = adminId
                };
                context.Diplomas.Add(diploma1);
            }

            var diploma2 = await context.Diplomas.FirstOrDefaultAsync(d => d.Title == "Data Science Fundamentals");
            if (diploma2 == null)
            {
                diploma2 = new Diploma
                {
                    Id = Guid.NewGuid(),
                    Title = "Data Science Fundamentals",
                    Description = "Covers statistics, Python, and machine learning basics.",
                    TotalQuizCount = 2,
                    Status = DiplomaStatus.Published,
                    AdminId = adminId,
                    CreatedBy = adminId
                };
                context.Diplomas.Add(diploma2);
            }

            await context.SaveChangesAsync();

            // ── Quizzes (idempotent) ──
            var quiz1 = await context.Quizzes.FirstOrDefaultAsync(q => q.Title == "HTML & CSS Basics")
                ?? new Quiz
                {
                    Id = Guid.NewGuid(),
                    Title = "HTML & CSS Basics",
                    DurationMinutes = 30,
                    PassScore = 60m,
                    MaxAttempts = 3,
                    Instructions = "Answer all questions. Each question carries equal marks.",
                    Status = QuizStatus.Published,
                    TotalQuestionsCache = 3,
                    DiplomaId = diploma1.Id,
                    CreatedBy = adminId
                };

            var quiz2 = await context.Quizzes.FirstOrDefaultAsync(q => q.Title == "JavaScript Essentials")
                ?? new Quiz
                {
                    Id = Guid.NewGuid(),
                    Title = "JavaScript Essentials",
                    DurationMinutes = 45,
                    PassScore = 70m,
                    MaxAttempts = 2,
                    Instructions = "Choose the best answer for each question.",
                    Status = QuizStatus.Published,
                    TotalQuestionsCache = 3,
                    DiplomaId = diploma1.Id,
                    CreatedBy = adminId
                };

            var quiz3 = await context.Quizzes.FirstOrDefaultAsync(q => q.Title == "Python Programming")
                ?? new Quiz
                {
                    Id = Guid.NewGuid(),
                    Title = "Python Programming",
                    DurationMinutes = 40,
                    PassScore = 65m,
                    MaxAttempts = 3,
                    Instructions = "Read each question carefully before answering.",
                    Status = QuizStatus.Published,
                    TotalQuestionsCache = 3,
                    DiplomaId = diploma2.Id,
                    CreatedBy = adminId
                };

            var quiz4 = await context.Quizzes.FirstOrDefaultAsync(q => q.Title == "Statistics & Probability")
                ?? new Quiz
                {
                    Id = Guid.NewGuid(),
                    Title = "Statistics & Probability",
                    DurationMinutes = 35,
                    PassScore = 60m,
                    MaxAttempts = 2,
                    Instructions = "Select the correct answer.",
                    Status = QuizStatus.Draft,
                    TotalQuestionsCache = 2,
                    DiplomaId = diploma2.Id,
                    CreatedBy = adminId
                };

            foreach (var q in new[] { quiz1, quiz2, quiz3, quiz4 })
                if (context.Entry(q).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
                    context.Quizzes.Add(q);

            await context.SaveChangesAsync();

            // ── Questions & Options for Quiz 1 (HTML & CSS) ──
            var q1_1 = CreateMcq(quiz1.Id, "What does HTML stand for?", 1, adminId,
                ("Hyper Text Markup Language", true, "HTML is the standard markup language for web pages."),
                ("High Tech Modern Language", false, null),
                ("Hyper Transfer Markup Language", false, null),
                ("Home Tool Markup Language", false, null));

            var q1_2 = CreateMcq(quiz1.Id, "Which CSS property is used to change text color?", 2, adminId,
                ("font-color", false, null),
                ("text-color", false, null),
                ("color", true, "The 'color' property sets the foreground color of text."),
                ("text-style", false, null));

            var q1_3 = CreateTrueFalse(quiz1.Id, "CSS stands for Cascading Style Sheets.", 3, true, adminId);

            // ── Questions & Options for Quiz 2 (JavaScript) ──
            var q2_1 = CreateMcq(quiz2.Id, "Which keyword declares a block-scoped variable in JavaScript?", 1, adminId,
                ("var", false, "var is function-scoped."),
                ("let", true, "let declares a block-scoped variable."),
                ("const", false, "const is block-scoped but immutable."),
                ("dim", false, null));

            var q2_2 = CreateTrueFalse(quiz2.Id, "JavaScript is a statically typed language.", 2, false, adminId);

            var q2_3 = CreateMcq(quiz2.Id, "What is the output of typeof null?", 3, adminId,
                ("'null'", false, null),
                ("'undefined'", false, null),
                ("'object'", true, "This is a well-known JavaScript quirk."),
                ("'boolean'", false, null));

            // ── Questions & Options for Quiz 3 (Python) ──
            var q3_1 = CreateMcq(quiz3.Id, "Which of the following is used to define a function in Python?", 1, adminId,
                ("function", false, null),
                ("def", true, "The 'def' keyword defines a function in Python."),
                ("func", false, null),
                ("lambda", false, "lambda creates anonymous functions, not named ones."));

            var q3_2 = CreateTrueFalse(quiz3.Id, "Python uses indentation to define code blocks.", 2, true, adminId);

            var q3_3 = CreateMcq(quiz3.Id, "What is the output of print(type([]))?", 3, adminId,
                ("<class 'tuple'>", false, null),
                ("<class 'dict'>", false, null),
                ("<class 'list'>", true, "[] creates an empty list."),
                ("<class 'set'>", false, null));

            // ── Questions for Quiz 4 (Statistics — Draft) ──
            var q4_1 = CreateMcq(quiz4.Id, "What is the mean of the data set {2, 4, 6, 8}?", 1, adminId,
                ("4", false, null),
                ("5", true, "Mean = (2+4+6+8)/4 = 5."),
                ("6", false, null),
                ("3", false, null));

            var q4_2 = CreateTrueFalse(quiz4.Id, "The median of an even-numbered data set is the average of the two middle values.", 2, true, adminId);

            // Add all questions to context
            context.MultipleChoiceQuestions.AddRange(
                q1_1.question, q1_2.question,
                q2_1.question, q2_3.question,
                q3_1.question, q3_3.question,
                q4_1.question);

            context.TrueFalseQuestions.AddRange(
                q1_3.question, q2_2.question,
                q3_2.question, q4_2.question);

            await context.SaveChangesAsync();

            // ── Student Enrollments ──
            var enrollment1 = new StudentDiploma
            {
                Id = Guid.NewGuid(),
                StudentId = student1Id,
                DiplomaId = diploma1.Id,
                Status = EnrollmentStatus.Active,
                CreatedBy = adminId
            };

            var enrollment2 = new StudentDiploma
            {
                Id = Guid.NewGuid(),
                StudentId = student1Id,
                DiplomaId = diploma2.Id,
                Status = EnrollmentStatus.Active,
                CreatedBy = adminId
            };

            var enrollment3 = new StudentDiploma
            {
                Id = Guid.NewGuid(),
                StudentId = student2Id,
                DiplomaId = diploma1.Id,
                Status = EnrollmentStatus.Active,
                CreatedBy = adminId
            };

            context.StudentDiplomas.AddRange(enrollment1, enrollment2, enrollment3);

            // ── Completed Attempt (student1 on quiz1) ──
            var attempt = new Attempt
            {
                Id = Guid.NewGuid(),
                StudentId = student1Id,
                QuizId = quiz1.Id,
                Status = AttemptStatus.Submitted,
                StartTime = DateTime.UtcNow.AddHours(-2),
                Deadline = DateTime.UtcNow.AddHours(-2).AddMinutes(30),
                SubmittedAt = DateTime.UtcNow.AddHours(-2).AddMinutes(20),
                TotalQuestions = 3,
                CorrectAnswers = 2,
                Score = 66.67m,
                Passed = true,
                CreatedBy = student1Id
            };

            context.Attempts.Add(attempt);
            await context.SaveChangesAsync();

            // Answers for the attempt (quiz1 questions)
            var answer1 = new Answer
            {
                Id = Guid.NewGuid(),
                AttemptId = attempt.Id,
                QuestionId = q1_1.question.Id,
                SelectedOptionId = q1_1.options[0].Id, // correct
                IsCorrect = true,
                CreatedBy = student1Id
            };

            var answer2 = new Answer
            {
                Id = Guid.NewGuid(),
                AttemptId = attempt.Id,
                QuestionId = q1_2.question.Id,
                SelectedOptionId = q1_2.options[0].Id, // wrong (font-color)
                IsCorrect = false,
                CreatedBy = student1Id
            };

            var answer3 = new Answer
            {
                Id = Guid.NewGuid(),
                AttemptId = attempt.Id,
                QuestionId = q1_3.question.Id,
                StudentAnswer = true, // correct
                IsCorrect = true,
                CreatedBy = student1Id
            };

            context.Answers.AddRange(answer1, answer2, answer3);
            await context.SaveChangesAsync();
        }

        // ── Helper: Create MCQ with options ──
        private static (MultipleChoiceQuestion question, List<QuestionOption> options) CreateMcq(
            Guid quizId, string text, int order, Guid adminId,
            params (string text, bool isCorrect, string? explanation)[] optionDefs)
        {
            var question = new MultipleChoiceQuestion
            {
                Id = Guid.NewGuid(),
                QuizId = quizId,
                QuestionText = text,
                OrderIndex = order,
                QuestionType = "MCQ",
                AllowMultipleCorrectAnswers = false,
                OptionsCount = optionDefs.Length,
                CreatedBy = adminId
            };

            var options = new List<QuestionOption>();
            for (int i = 0; i < optionDefs.Length; i++)
            {
                var opt = new QuestionOption
                {
                    Id = Guid.NewGuid(),
                    MCQQuestionId = question.Id,
                    OptionText = optionDefs[i].text,
                    IsCorrect = optionDefs[i].isCorrect,
                    Explanation = optionDefs[i].explanation,
                    OrderIndex = i + 1,
                    CreatedBy = adminId
                };
                options.Add(opt);
            }

            question.Options = options;
            return (question, options);
        }

        // ── Helper: Create True/False question ──
        private static (TrueFalseQuestion question, List<QuestionOption> options) CreateTrueFalse(
            Guid quizId, string text, int order, bool correctAnswer, Guid adminId)
        {
            var question = new TrueFalseQuestion
            {
                Id = Guid.NewGuid(),
                QuizId = quizId,
                QuestionText = text,
                OrderIndex = order,
                QuestionType = "TrueFalse",
                CorrectAnswer = correctAnswer,
                CreatedBy = adminId
            };

            return (question, new List<QuestionOption>());
        }
    }
}
