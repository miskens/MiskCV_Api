using Microsoft.AspNetCore.Identity;

namespace MiskCv_Api.Data;

public static class SeedRolesAndAdmin
{
    public async static void Seed(WebApplication app, WebApplicationBuilder builder)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await userManager.FindByNameAsync("misken") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "misken",
                    Email = "miskens@hotmail.com",
                };

                await userManager.CreateAsync(user, builder.Configuration["UserAdmin:pwd"]!);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
