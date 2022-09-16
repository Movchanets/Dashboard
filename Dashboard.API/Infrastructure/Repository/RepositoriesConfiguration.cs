using Dashboard.Data.Data.Classes;
using Dashboard.Data.Data.Interfaces;

namespace Dashboard.API.Infrastructure.Repository
{
    public class RepositoriesConfiguration
    {
        public static void Config(IServiceCollection services)
        {
           services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
