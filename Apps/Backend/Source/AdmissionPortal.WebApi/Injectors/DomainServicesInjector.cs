using AdmissionPortal.Application;
using AdmissionPortal.Domain;
using AdmissionPortal.Domain.Abstractions;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class DomainServicesInjector
    {
        public static void InjectDomainServices(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectDomainServices");

            var applicationAssembly = typeof(IApplicationMarker).Assembly;
            var domainAssembly = typeof(IDomainMarker).Assembly;

            services.Scan(scan => scan
                .FromAssemblies([applicationAssembly, domainAssembly])
                .AddClasses(classes => classes.AssignableTo<IDomainService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            Console.WriteLine("Finished InjectDomainServices");
        }
    }
}