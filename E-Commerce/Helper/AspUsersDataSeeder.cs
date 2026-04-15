using E_Commerce.Data;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Helper
{
    public static class AspUsersDataSeeder
    {
        public static async Task SeedAdminAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new ApplicationRole
                {
                    Name = "Admin",
                    Description = "Administrator role with full permissions",
                };
                await roleManager.CreateAsync(adminRole);
            }

            var adminEmail = "admin@gmail.com";
            
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var adminPassword = "P@ssw0rd";
                admin = new ApplicationUser
                {
                    FirstName = "Abdallah",
                    LastName = "Eldesouky",
                    UserName = "Remando",
                    Email = adminEmail
                };
                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        public static async Task SeedCustomerRoleAsync(RoleManager<ApplicationRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Customer"))
            {
                var customerRole = new ApplicationRole
                {
                    Name = "Customer",
                    Description = "Customer role with limited permissions",
                };
                await roleManager.CreateAsync(customerRole);
            }
        }
    }
}
