using Dashboard.API.Infrastructure.AutoMapper;
using Dashboard.API.Infrastructure.Repositories;
using Dashboard.API.Infrastructure.Services;
using Dashboard.Data.Data.Context;
using Dashboard.Data.Initializer;
using Dashboard.Services.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add dababase context
builder.Services.AddDbContext<AppDbContext>();

// Add AutoMapper configuration 
AutoMapperConfiguration.Config(builder.Services);

//Add Services configuration
ServicesConfiguration.Config(builder.Services);

// Add Repositories configuration
RepositoriesConfiguration.Config(builder.Services);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = false,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt => {
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options => options
	.WithOrigins(new[] { "http://localhost:3000" })
	.AllowAnyHeader()
	.AllowCredentials()
);
app.UseAuthentication();
app.UseAuthorization();



app.MapRazorPages();
app.MapControllers();

await AppDbInitializer.Seed(app);
app.Run();


