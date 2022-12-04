using BackendFinalProjectEduHome.DAL.Entities;
using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BackendFinalProjectEduHome.DAL
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduHomeDbContext _dbContext;
        private readonly AdminUser _adminUser;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _adminUser = serviceProvider.GetService<IOptions<AdminUser>>().Value;
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _dbContext = serviceProvider.GetRequiredService<EduHomeDbContext>();
        }

        public async Task SeedData()
        {
            await _dbContext.Database.MigrateAsync();

            var role = Constants.AdminRole;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        //logging
                        Console.WriteLine(error.Description);
                    }
                }
            }

            var activeUser = await _userManager.FindByNameAsync(_adminUser.UserName);

            if (activeUser != null) return;

            var createUser = await _userManager.CreateAsync(new User
            {
                UserName = _adminUser.UserName,
                Email = _adminUser.Email,
            }, _adminUser.Password);

            if (!createUser.Succeeded)
            {
                foreach (var error in createUser.Errors)
                {
                    //logging
                    Console.WriteLine(error.Description);
                }
            }
            else
            {
                var existCreateUser = await _userManager.FindByNameAsync(_adminUser.UserName);

                await _userManager.AddToRoleAsync(existCreateUser, Constants.AdminRole);
            }
        }
    }
}
