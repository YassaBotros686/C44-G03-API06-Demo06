using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var User = new ApplicationUser
                    {
                        DisplayName = "Marawan Ali",
                        UserName = "MarawanAli",
                        Email = "marawanali190@gmail.com",
                        PhoneNumber = "01022223333",
                    };

                    var User2 = new ApplicationUser
                    {
                        DisplayName = "Mohamed Ahmed",
                        UserName = "MohamedAhmed",
                        Email = "mohamedahmed@gmail.com",
                        PhoneNumber = "01044445555",
                    };

                    await _userManager.CreateAsync(User, "P@ssw0rd");
                    await _userManager.CreateAsync(User2, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User, "Admin");
                    await _userManager.AddToRoleAsync(User2, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while seeding identity data : {ex}");
            }
        }
    }
}
