using Microsoft.AspNetCore.Identity;
using Rental4You.Models;

namespace Rental4You.Data
{
    public enum Roles
    {
        Admin,
        Client,
        Employee,
        Manager
    }
    public class Initialization
    {
        public static async Task PopulateData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Populate default Roles
            foreach (Roles Role in Enum.GetValues(typeof(Roles)))
            {
                await roleManager.CreateAsync(new IdentityRole(Role.ToString()));
            }
            // Populate database with default admin
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                FirstName = "Administrator",
                LastName = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin...123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
            }
        }
    }
}
