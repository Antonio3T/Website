using Microsoft.AspNetCore.Identity;

namespace Website.Data
{
    public static class SeedRoles
    {
        public static void Seed(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Any() == false)
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("Employee")).Wait();
                roleManager.CreateAsync(new IdentityRole("Client")).Wait();
            }
        }
    }
}