using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.Register
{
    public class PostRegisterHandler : IRequestHandler<PostRegisterCommand, Result<RegisterResponse>> 
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostRegisterHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<RegisterResponse>> Handle(PostRegisterCommand registerCommand, CancellationToken cancellationToken)
        {
            var user = new AuthAppUser()
            {
                FullName = registerCommand.RegisterDTO.full_name,
                Email = registerCommand.RegisterDTO.email,     
                UserName = registerCommand.RegisterDTO.full_name
            };

            var result = await _userManager.CreateAsync(user, registerCommand.RegisterDTO.password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList();

                return Result<RegisterResponse>.Failure(string.Join(", ", errors));

            }

            var response = new RegisterResponse()
            {
                email = user.Email! ,
                full_name = user.FullName!,
                token = "token"
            };


            return Result<RegisterResponse>.Success(response);
        }


    }
}

