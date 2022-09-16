using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Data.Initializer
{

    public class AppDbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (IServiceScope? serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager.FindByNameAsync("master").Result == null)
                {
                    AppUser user = new AppUser()
                    {
                        Email = "admin@email.com",
                        NormalizedEmail = "ADMIN@EMAIL.COM",
                        UserName = "master",
                        NormalizedUserName = "ADMIN@EMAIL.COM",
                        EmailConfirmed = true,
                        Name = "Admin",
                        Surname = "Adminovich",
                        Address = "None",
                    };



                    context.Roles.AddRange(
                         new IdentityRole()
                         {
                             Name = "Administrators",
                             NormalizedName = "ADMINISTRATORS"
                         }
                     );



                    await context.SaveChangesAsync();



                    IdentityResult result = userManager.CreateAsync(user, "Qwerty-1").Result;



                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Administrators").Wait();
                    }
                }
            }

        }

    }
}