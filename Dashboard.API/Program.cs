using Dashboard.API.Infrastructure.AutoMapper;
using Dashboard.API.Infrastructure.Repository;
using Dashboard.API.Infrastructure.Services;
using Dashboard.Data.AutoMapper;
using Dashboard.Data.Data.Classes;
using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models;
using Dashboard.Data.Initializer;
using Dashboard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<EmailService>();
//add services configuration 
ServicesConfiguration.Config(builder.Services);
//add repository configuration
RepositoriesConfiguration.Config(builder.Services);
builder.Services.AddScoped<IUserRepository, UserRepository>();
//add jwt
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters

    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration.GetSection("AuthSettings").GetValue<string>("Issuer"),
        ValidAudience = builder.Configuration.GetSection("AuthSettings").GetValue<string>("Audience"),
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthSettings").GetValue<string>("Key"))),
        ValidateIssuerSigningKey = true
    };
});
//add razorpages
builder.Services.AddRazorPages();
//add automapper
AutoMapperConfiguration.Config(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
await AppDbInitializer.Seed(app);
app.Run();
