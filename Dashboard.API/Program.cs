using Dashboard.API.Infrastructure.AutoMapper;
using Dashboard.API.Infrastructure.Repository;
using Dashboard.API.Infrastructure.Services;
using Dashboard.Data.Initializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




//add services configuration 
ServicesConfiguration.Config(builder.Services);
//add repository configuration
RepositoriesConfiguration.Config(builder.Services);

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
