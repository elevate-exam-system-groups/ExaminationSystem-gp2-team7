
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExaminationSystem.Common;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace ExaminationSystem.Features.Auth.Register


{
    public class SeedIdentityHandler : IRequestHandler<SeedIdentityCommand, Result<RegisterResponse>>
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;


        public SeedIdentityHandler(
                RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager ,
             IOptions<JwtSettings> jwtSettings)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;

        }
        public async Task<Result<RegisterResponse>> Handle(SeedIdentityCommand request, CancellationToken cancellationToken)
        {
            // 1. Seed Roles
            string[] roles = { "Admin", "Student" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            // 2. Seed Admin User
            string adminEmail = "admin@mail.com";
            string password = "Admin@123";

            var admin = await _userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FullName = adminEmail,
                    Email = adminEmail ,
                    UserName = adminEmail

                };

                var result = await _userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList();
                    return Result<RegisterResponse>.Failure(string.Join(", ", errors));
                }


            }
            // Add user to "Student" role
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }
            await _userManager.AddToRoleAsync(admin, "Admin");

            var user_roles = await _userManager.GetRolesAsync(admin);
            var accessToken = GenerateAccessToken(admin, user_roles);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(admin, refreshToken);

            var response = new RegisterResponse()
            {
                email = admin.Email!,
                full_name = admin.FullName!,
                access_token = new RefreshToken
                {
                    Token = accessToken,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes)
                },
                refresh_token = new RefreshToken
                {
                    Token = refreshToken,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
                }
            };


            return Result<RegisterResponse>.Success(response);
        }



        // Generate JWT access token
        private string GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                 {
                 new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Email, user.Email !),
                 new Claim(ClaimTypes.Name, user.UserName ?? "")
                 };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken(
             issuer: _jwtSettings.Issuer,
             audience: _jwtSettings.Audience,
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }




        // Generate refresh token (random string)
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }


        // Save refresh token to database
        private async Task SaveRefreshTokenAsync(ApplicationUser user, string token)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
            };
            user.RefreshTokens.Add(refreshToken!);

            await _userManager.UpdateAsync(user);
        }
    }
    }

