using AdmissionPortal.Domain.Infrastructure;
using AdmissionPortal.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class DbContextInjector
    {
        public static void InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("Started InjectDbContext");

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AdmissionPortalDbContext>());

            services.AddDbContext<AdmissionPortalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
            });

            Console.WriteLine("Finished InjectDbContext");
        }
    }
}
