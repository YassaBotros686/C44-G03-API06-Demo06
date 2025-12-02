using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
            {
                return Error.InvalidCredentials("Invalid Email Or Password !");
            }
            var PasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!PasswordValid)
            {
                return Error.InvalidCredentials("Password Invalid");
            }
            return new UserDTO(User.Email!, User.DisplayName, "Token");
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber
            };

            var IdentityResult = await _userManager.CreateAsync(User, registerDTO.Password);

            if (IdentityResult.Succeeded)
                return new UserDTO(User.Email, User.DisplayName, "Token");

            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
        }
    }
}
