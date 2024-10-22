using DocLink.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocLink.Data.SeedData;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        var roles = new List<string> { "admin", "member", "doctor" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed Default Admin User
        var defaultUser = new AppUser
        {
            UserName = "mehemmed05.aliyev@gmail.com",
            Name = "Mohammed",
            Surname = "Aliyev",
            Email = "mehemmed05.aliyev@gmail.com",
            EmailConfirmed = true
        };

        if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
        {
            var result = await userManager.CreateAsync(defaultUser, "Mehemmed@2005");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(defaultUser, "admin");
            }
        }
    }
}

