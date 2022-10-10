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
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager.FindByNameAsync("master").Result == null)
                {
                    AppUser user = new AppUser()
                    {
                        UserName = "master",
                        Email = "master@email.com",
                        EmailConfirmed = true,
                        Name = "Admin",
                        Surname = "Adminovich",
                        
                    };
                    AppUser user2 = new AppUser()
                    {
                        UserName = "slavik",
                        Email = "slavik@email.com",
                        EmailConfirmed = true,
                        Name = "Slavik",
                        Surname = "Movchanets",

                    };
                    context.Roles.AddRange(
                        new IdentityRole()
                        {
                            Name = "Administrators",
                            NormalizedName = "ADMINISTRATORS"
                        }
                        ,
                        new IdentityRole()
                        {
                            Name = "Users",
                            NormalizedName = "USERS"

                        }); ;

                    await context.SaveChangesAsync();

                    IdentityResult result = userManager.CreateAsync(user, "Qwerty-1").Result;
                    IdentityResult result2 = userManager.CreateAsync(user2, "Qwerty-1").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Administrators").Wait();
                    }
                    if (result2.Succeeded)
                    {
                        userManager.AddToRoleAsync(user2, "Users").Wait();
                    }
                }
            }
               
        }
    }
}
