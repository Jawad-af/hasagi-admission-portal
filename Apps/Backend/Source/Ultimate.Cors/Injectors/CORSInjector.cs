using Microsoft.Extensions.DependencyInjection;

namespace Ultimate.Cors.Injectors
{
    public static class CORSInjector
    {
        public static void InjectCORS(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectCROS");

            _ = services.AddCors(options =>
            {
                options.AddPolicy(name: "LocalCorsPolicy",
                                  builder =>
                                  {
                                      _ = builder
                                            .AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });

            Console.WriteLine("Finished InjectCROS");
        }
    }
}
