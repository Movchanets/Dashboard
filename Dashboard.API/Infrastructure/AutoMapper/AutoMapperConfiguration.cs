using Dashboard.Data.Data.AutoMapper;

namespace Dashboard.API.Infrastructure.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperUserProfile));
        }
    }
}
