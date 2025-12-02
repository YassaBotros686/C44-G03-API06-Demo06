using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IAuthenticationService
    {
        // DTOs
        // LoginDTO
        // RegisterDTO
        // UserDTO

        // Login
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);

        // Register
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);

    }
}
