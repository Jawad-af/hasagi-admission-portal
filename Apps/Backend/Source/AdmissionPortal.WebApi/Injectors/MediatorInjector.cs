using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using Ultimate.Mediator;
using Ultimate.Mediator.Interfaces;
using Ultimate.Mediator.Pipelines;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class MediatorInjector
    {
        public static void InjectMediator(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectMediator");

            // Register mediator
            services.AddTransient<IMediator, Mediator>();

            // Register handlers
            services.RegisterIdentityHandlers();

            // Register pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            Console.WriteLine("Finished InjectMediator");
        }

        private static void RegisterIdentityHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<LoginCommand, AuthenticationResponseDto>, LoginCommandHandler>();
            services.AddTransient<ICommandHandler<SignupCommand, AuthenticationResponseDto>, SignupCommandHandler>();
            services.AddTransient<ICommandHandler<RefreshTokenCommand, AuthenticationResponseDto>, RefreshTokenCommandHandler>();
        }
    }
}
