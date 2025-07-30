using Microsoft.AspNetCore.Builder;

namespace Ultimate.Cors.Configurators
{
    public static class CORSConfigurator
    {
        public static void ConfigureCORS(this WebApplication app)
        {
            Console.WriteLine("Started ConfigureCORS");

            _ = app.UseCors("LocalCorsPolicy");

            Console.WriteLine("Finished ConfigureCORS");
        }
    }
}
