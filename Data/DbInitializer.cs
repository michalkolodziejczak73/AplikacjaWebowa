using Microsoft.AspNetCore.Identity;

namespace AplikacjaWebowa.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeRolesAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Ankieter", "Respondent" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string ankieterEmail = "michal@test1.pl";

            var ankieter = await userManager.FindByEmailAsync(ankieterEmail);

            if (ankieter != null &&
                !await userManager.IsInRoleAsync(ankieter, "Ankieter"))
            {
                await userManager.AddToRoleAsync(ankieter, "Ankieter");
            }
        }
    }
}