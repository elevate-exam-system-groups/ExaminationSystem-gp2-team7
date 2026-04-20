using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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

            builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings"));

            // MediatR
            //builder.Services.AddMediatR(cfg =>
            //    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            // FluentValidation
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


            // In-Memory Caching
            builder.Services.AddMemoryCache();

            // Email Service
            builder.Services.AddScoped<ExaminationSystem.Contracts.IEmailService, ExaminationSystem.Repositories.EmailService>();

            // Unit of Work
            builder.Services.AddScoped<ExaminationSystem.Contracts.IUnitOfWork, ExaminationSystem.Repositories.UnitOfWork>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
