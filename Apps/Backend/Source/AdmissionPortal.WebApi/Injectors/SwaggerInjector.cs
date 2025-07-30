using Microsoft.OpenApi.Models;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class SwaggerInjector
    {
        public static void InjectSwagger(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectSwagger");

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Admission Portal API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer eyJhbGciOiJI...')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            Console.WriteLine("Finished InjectSwagger");
        }
    }
}