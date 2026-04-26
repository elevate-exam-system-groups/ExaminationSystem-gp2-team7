using System.Text;
using ExaminationSystem.Common;
using ExaminationSystem.Common.Behaviors;
using ExaminationSystem.Contracts;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers;
using ExaminationSystem.Features.Attempts.Services;
using ExaminationSystem.Features.Attempts.SubmitQuiz;
using ExaminationSystem.Features.Auth.Register;
using ExaminationSystem.Models;
using ExaminationSystem.Repositories;
using ExaminationSystem.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // 1️⃣ DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("ExaminationSystem")));

            // 2️⃣ Identity    
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 3️⃣ JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(SeedIdentityHandler).Assembly
                );
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            // In-Memory Caching
            builder.Services.AddMemoryCache();

            // Email Service
            builder.Services.AddScoped<IEmailService, ExaminationSystem.Repositories.EmailService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Examination System API", Version = "v1" });

                // 1. Define the Security Scheme (This adds the "Authorize" button in Swagger UI)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
                });

                // 2. Apply the Security Requirement Globally (This ensures Swagger actually sends the token with your requests)
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Feature Readers
            builder.Services.AddScoped<AttemptResultsReader>();
            builder.Services.AddScoped<ExaminationSystem.Features.Attempts.SubmitQuiz.Helpers.SubmitQuizReader>();
            builder.Services.AddScoped<ExaminationSystem.Features.Questions.Commands.CreateQuestion.Helpers.CreateQuestionReader>();
            builder.Services.AddScoped<ExaminationSystem.Features.Questions.Commands.UpdateQuestion.Helpers.UpdateQuestionReader>();
            builder.Services.AddScoped<ExaminationSystem.Features.Questions.Commands.DeleteQuestion.Helpers.DeleteQuestionReader>();
            builder.Services.AddScoped<ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail.AttemptDetailReader>();

            // Timer Enforcement
            builder.Services.AddScoped<IAttemptAutoSubmitService, AttemptAutoSubmitService>();
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AttemptDeadlineBehavior<,>));


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SeedIdentityCommand());


                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            }

            // Seed mock data for testing
            await DataSeeder.SeedMockDataAsync(app.Services);


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == 404)
                {
                    response.ContentType = "application/json";

                    await response.WriteAsync("""
                                             {
                                                 "status": 404,
                                                 "title": "Not Found",
                                                 "message": "The requested resource was not found"
                                             }
                                             """);
                }
            });

            app.MapControllers();

            app.Run();
        }
    }
}
