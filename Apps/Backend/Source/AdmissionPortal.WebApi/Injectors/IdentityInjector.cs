using AdmissionPortal.Application.Options;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Enums.Identity.Authentication;
using AdmissionPortal.Infrastructure.Persistance;
using AdmissionPortal.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class IdentityInjector
    {
        public static IServiceCollection InjectIdentity(this IServiceCollection services, IConfiguration configurations)
        {
            AccessTokenOptions? accessTokenOptions = configurations.GetSection(nameof(AccessTokenOptions)).Get<AccessTokenOptions>();
            RefreshTokenOptions? refreshTokenOptions = configurations.GetSection(nameof(RefreshTokenOptions)).Get<RefreshTokenOptions>();

            if (accessTokenOptions == null ||
                string.IsNullOrEmpty(accessTokenOptions.Issuer) ||
                string.IsNullOrEmpty(accessTokenOptions.Audience) ||
                string.IsNullOrEmpty(accessTokenOptions.Secret))
            {
                Console.WriteLine("ERR: AccessTokenOptions missing");

                throw new InvalidOperationException(
                       "Missing or incomplete configuration for 'AccessTokenOptions' section.");
            }

            if (refreshTokenOptions == null ||
                string.IsNullOrEmpty(refreshTokenOptions.Issuer) ||
                string.IsNullOrEmpty(refreshTokenOptions.Audience) ||
                string.IsNullOrEmpty(refreshTokenOptions.Secret))
            {
                Console.WriteLine("ERR: RefreshTokenOptions missing");

                throw new InvalidOperationException(
                       "Missing or incomplete configuration for 'RefreshTokenOptions' section.");
            }

            TokenValidationParameters accessTokenValidationParameters = new()
            {
                ValidIssuer = accessTokenOptions.Issuer,
                ValidAudience = accessTokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessTokenOptions.Secret)),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true
            };

            services.AddSingleton(accessTokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(TokenType.Access.ToString(), options =>
                {
                    options.TokenValidationParameters = accessTokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = ctx =>
                        {
                            var type = ctx.Principal?.FindFirst("token_type")?.Value;
                            if (type != TokenType.Access.ToString())
                            {
                                ctx.Fail("Invalid token type");
                            }
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddJwtBearer(TokenType.Refresh.ToString(), options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = refreshTokenOptions.Issuer,
                        ValidAudience = refreshTokenOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenOptions.Secret)),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = ctx =>
                        {
                            var type = ctx.Principal?.FindFirst("token_type")?.Value;
                            if (type != TokenType.Refresh.ToString())
                            {
                                ctx.Fail("Invalid token type");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AdmissionPortalDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}