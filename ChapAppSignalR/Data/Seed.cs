using ChapAppSignalR.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ChapAppSignalR.Data
{
    public class Seed
    {

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                
                
                //string adminUserEmail = "teddysmithdeveloper@gmail.com";

                //var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                //if (adminUser == null)
                //{
                //    var newAdminUser = new AppUser()
                //    {
                //        UserName = "teddysmithdev",
                //        Email = adminUserEmail,
                //        EmailConfirmed = true,
                //        Adress = new Address()
                //        {
                //            Street = "123 Main St",
                //            City = "Charlotte",
                //            State = "NC"
                //        }
                //    };
                //    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                //    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                //}

                //string appUserEmail = "user@etickets.com";

                //var appUser = await userManager.FindByEmailAsync(appUserEmail);
                //if (appUser == null)
                //{
                //    var newAppUser = new AppUser()
                //    {
                //        UserName = "app-user",
                //        Email = appUserEmail,
                //        EmailConfirmed = true,
                //        Adress = new Address()
                //        {
                //            Street = "123 Main St",
                //            City = "Charlotte",
                //            State = "NC"
                //        }
                //    };
                //    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                //    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                //}
            }
        }
    }
}
