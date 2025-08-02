using AdmissionPortal.Application;
using Wolverine;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class MediatRInjector
    {
        public static void InjectWolverine(this IHostBuilder builder)
        {
            Console.WriteLine("Started InjectWolverine");

            builder.UseWolverine(opts =>
            {
                opts.Durability.Mode = DurabilityMode.MediatorOnly;
                opts.Discovery.IncludeAssembly(typeof(IApplicationMarker).Assembly);
            });

            Console.WriteLine("Finished InjectWolverine");
        }
    }
}
