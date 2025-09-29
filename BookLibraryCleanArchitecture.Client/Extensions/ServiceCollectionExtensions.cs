using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Interfaces;
using BookLibraryCleanArchitecture.Infrastructure.Interfaces;
using BookLibraryCleanArchitecture.Infrastructure.Mappers;
using BookLibraryCleanArchitecture.Infrastructure.Middlewares;
using BookLibraryCleanArchitecture.Infrastructure.Processors;
using BookLibraryCleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BookLibraryCleanArchitecture.Client.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            // Register client-specific services here
            // e.g., services.AddTransient<IMyClientService, MyClientService>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application-specific services here
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            // Register infrastructure-specific services here
            // e.g., services.AddTransient<IMyInfrastructureService, MyInfrastructureService>();
            services.AddScoped<IAuthenticationProcessor, AuthenticationProcessor>();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddProblemDetails(); // Enables RFC-compliant error formatting
            services.AddSingleton<IProblemDetailsService, BookLibraryProblemDetailsService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddSingleton<IExceptionProblemDetailsMapper, AuthenticationExceptionMapper>();
            services.AddSingleton<IExceptionProblemDetailsMapper, TokenGenerationExceptionMapper>();
            services.AddSingleton<IExceptionProblemDetailsMapper, RegistrationExceptionMapper>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Register domain-specific services here
            // e.g., services.AddTransient<IMyDomainService, MyDomainService>();
            return services;
        }

        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddClientServices();
            services.AddApplicationServices();
            services.AddInfrastructureServices();
            services.AddDomainServices();
            services.AddAuthentication(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer( options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JwtOptionsSection).Get<JwtOptions>() 
                ?? throw new InvalidOperationException("JWt configuration is missing, please add the configuration in appsetting.json");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = JwtTokenGenerator.GetSymmetricSecurityKey(jwtOptions.SecretKey),
                };

                /* if we store the JWT in the cookie, we need to read it from there */
                /* Custom logic to read token from cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["accessToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                */
            });

            return services;
        }
    }
}
