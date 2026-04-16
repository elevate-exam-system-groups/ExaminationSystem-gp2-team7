using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.Features.Auth.Register
{
    public class PostRegisterHandler : IRequestHandler<PostRegisterCommand, Result<RegisterResponse>> 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly JwtSettings _jwtSettings;
        public PostRegisterHandler(UserManager<ApplicationUser> userManager ,
                RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _roleManager = roleManager;
        }

        public async Task<Result<RegisterResponse>> Handle(PostRegisterCommand registerCommand, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser() 
            { 
                FullName = registerCommand.RegisterDTO.full_name,
                Email = registerCommand.RegisterDTO.email,
                UserName = registerCommand.RegisterDTO.email
            };

            var result = await _userManager.CreateAsync(user, registerCommand.RegisterDTO.password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList();
                return Result<RegisterResponse>.Failure(string.Join(", ", errors));
            }

            // Add user to "User" role
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }
            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = GenerateAccessToken(user, roles);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(user, refreshToken);

            var response = new RegisterResponse()
            {
                email = user.Email! ,
                full_name = user.FullName!,
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
             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes),signingCredentials: credentials);
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



        //public async Task CleanupExpiredTokensAsync()
        //{
        //    var users = _userManager.Users.ToList();
        //    foreach (var user in users )
        //    {
        //        var expiredTokens = user.RefreshTokens
        //        .Where(rt => rt.IsExpired)
        //        .ToList();
        //        foreach (var token in expiredTokens)
        //        {
        //            user.RefreshTokens.Remove(token);
        //        }
        //        if (expiredTokens.Any())
        //            await _userManager.UpdateAsync(user);
        //    }
        //}
    }



}

