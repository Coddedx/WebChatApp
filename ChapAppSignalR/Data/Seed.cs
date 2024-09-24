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
                //Uygulamaya yeni kullanıcılar eklemek. Var olan kullanıcılara roller atamak.Kullanıcı bilgilerini doğrulamak veya parolasını değiştirmek. için userManager kullanılır yapmıcaksan gerek yok 
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            }
        }
    }
}
