using AdmissionPortal.Application.Options;

namespace AdmissionPortal.WebApi.Configurators
{
    public static class JwtOptionsConfigurator
    {
        public static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {
            Console.Write("Started ConfigureJwtOptions");

            services.Configure<AccessTokenOptions>(configuration.GetSection("AccessTokenOptions"));
            services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshTokenOptions"));

            Console.Write("Finished ConfigureJwtOptions");
        }
    }
}