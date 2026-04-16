using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Register;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.Features.Auth.Login
{
    public class PostLoginHandler : IRequestHandler<PostLoginCommand, Result<LoginResponse>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public PostLoginHandler(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<LoginResponse>> Handle
            (PostLoginCommand loginCommand, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(loginCommand.LoginDTO.email);

            if (user is null)
            {
                return Result<LoginResponse>.Failure("User not found", StatusCodes.Status404NotFound);

            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginCommand.LoginDTO.password);
            if (!passwordValid)
            {
                return Result<LoginResponse>.Failure("Invalid password", StatusCodes.Status401Unauthorized);
            }

            // Here you would typically generate a JWT token or similar for the authenticated user

            // Add user to "User" role
            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = GenerateAccessToken(user, roles);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(user, refreshToken);

            var response = new LoginResponse()
            {
                email = user.Email!,
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
            return Result<LoginResponse>.Success(response);
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
            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);
        }



    }
}

