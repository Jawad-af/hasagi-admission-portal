using AdmissionPortal.Application.Services.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Infrastructure.Persistance;
using AdmissionPortal.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class IdentityInjector
    {
        public static IServiceCollection InjectIdentity(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AdmissionPortalDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}