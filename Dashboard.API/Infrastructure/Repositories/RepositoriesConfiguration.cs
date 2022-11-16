using Dashboard.Data.Data.Classes;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Services;

namespace Dashboard.API.Infrastructure.Repositories
{
    public class RepositoriesConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            // Add IUserRepository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
        }
    }
}
