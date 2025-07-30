using Microsoft.Extensions.DependencyInjection;
using Ultimate.Exceptions.Middleware;

namespace Ultimate.Exceptions.Injectors
{
    public static class GlobalExceptionMiddlewareInjector
    {
        public static void InjectGlobalExceptionMiddleware(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectGlobalExceptionMiddleware");

            //_ = services.AddExceptionHandler<GlobalExceptionHandler>();
            //_ = services.AddProblemDetails();

            Console.WriteLine("Finished InjectGlobalExceptionMiddleware");
        }
    }
}
