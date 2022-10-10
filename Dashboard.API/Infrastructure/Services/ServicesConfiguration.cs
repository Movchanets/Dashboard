using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dashboard.API.Infrastructure.Services
{
    public class ServicesConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            // Add user service
            services.AddTransient<UserService>();

            // Add email service
            services.AddTransient<EmailService>();

            // Add jwt service
            services.AddTransient<JwtService>();

            // Add RazorPages
            services.AddRazorPages();

            // Add services to the container.
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
            services.AddSwaggerGen();


            // Add Identity user and roles
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        }
    }
}
