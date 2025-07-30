using AdmissionPortal.Application;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class MediatRInjector
    {
        public static void InjectMediatR(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectMediatR");

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly);
            });

            Console.WriteLine("Finished InjectMediatR");
        }
    }
}
