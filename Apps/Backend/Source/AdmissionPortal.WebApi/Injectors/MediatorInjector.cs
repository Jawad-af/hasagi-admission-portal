using AdmissionPortal.Application;
using Wolverine;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class MediatorInjector
    {
        public static void InjectMediator(this IHostBuilder builder)
        {
            Console.WriteLine("Started InjectMediator");

            builder.UseWolverine(opts =>
            {
                opts.Discovery.IncludeAssembly(typeof(IApplicationMarker).Assembly);
                opts.Durability.Mode = DurabilityMode.MediatorOnly;
            });

            //// Register mediator
            //services.AddTransient<IMediator, Mediator>();

            //// Register handlers
            //services.RegisterIdentityHandlers();

            //// Register pipeline behaviors
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            Console.WriteLine("Finished InjectMediator");
        }

        private static void RegisterIdentityHandlers(this IServiceCollection services)
        {
            //services.AddTransient<ICommandHandler<LoginCommand, AuthenticationResponseDto>, LoginCommandHandler>();
            //services.AddTransient<ICommandHandler<SignupCommand, AuthenticationResponseDto>, SignupCommandHandler>();
            //services.AddTransient<ICommandHandler<RefreshTokenCommand, AuthenticationResponseDto>, RefreshTokenCommandHandler>();
        }
    }
}
