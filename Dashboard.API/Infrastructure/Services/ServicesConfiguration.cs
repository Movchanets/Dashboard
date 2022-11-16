using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Dashboard.API.Infrastructure.Services
{
    public class ServicesConfiguration
    {
        public static void Config(IServiceCollection services)
        {
			// Add user service
			services.AddTransient<UserService>();
			services.AddTransient<BlogService>();

			// Add email service
			services.AddTransient<EmailService>();

			// Add jwt service
			services.AddTransient<JwtService>();

			// Add RazorPages
			services.AddRazorPages();

			// Add services to the container.
			services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = "Dashboard API", Version = "v1" });
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
			}); ;


			// Add Identity user and roles
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
			
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
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

