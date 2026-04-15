using ExaminationSystem.Common;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.Login
{
    public class PostLoginHandler : IRequestHandler<PostLoginCommand, Result<LoginResponse>>
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public PostLoginHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

            return Result<LoginResponse>.Success(new LoginResponse
            {
                email = user.Email!,
                token = ""
            });

        }
    }
}
