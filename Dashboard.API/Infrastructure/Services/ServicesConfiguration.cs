using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.API.Infrastructure.Services
{
    public class ServicesConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<AppDbContext>();
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddTransient<UserService>();
            services.AddTransient<EmailService>();
            //add razorpages
            services.AddRazorPages();
            services.AddTransient<JwtService>();
        }
    }
}
