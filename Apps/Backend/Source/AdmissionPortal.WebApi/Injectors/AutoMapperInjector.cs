using AutoMapper;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class AutoMapperInjector
    {
        public static void InjectAutoMapper(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectAutoMapper");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var assembly in assemblies)
                {
                    cfg.AddMaps(assembly);
                }
            }, loggerFactory);

            services.AddSingleton(config.CreateMapper());

            Console.WriteLine("Finished InjectAutoMapper");
        }
    }
}
