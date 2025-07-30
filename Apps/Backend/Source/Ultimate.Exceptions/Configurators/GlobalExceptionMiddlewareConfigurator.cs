using Microsoft.AspNetCore.Builder;
using Ultimate.Exceptions.Middleware;

namespace Ultimate.Exceptions.Configurators
{
    public static class GlobalExceptionMiddlewareConfigurator
    {
        public static void ConfigureGlobalExceptionMiddleware(this WebApplication app)
        {
            Console.WriteLine("Started ConfigureGlobalExceptionMiddleware");

            _ = app.UseMiddleware<GlobalExceptionMiddleware>();

            Console.WriteLine("Finished ConfigureGlobalExceptionMiddleware");
        }
    }
}
