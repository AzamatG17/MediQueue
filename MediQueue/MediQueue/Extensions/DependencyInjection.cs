using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using MediQueue.Infrastructure.Persistence;
using MediQueue.Infrastructure.Persistence.Repositories;
using MediQueue.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MediQueue.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabaseContext(services, configuration);
            AddAuthentication(services, configuration);
            AddAuthorization(services);
            AddServices(services);
            AddRepositories(services);
            AddAutoMapper(services);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }

        private static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MediQueueConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Invalid connection string.");
            }

            services.AddDbContext<MediQueueDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAccountService, AccountService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty-cookies"];

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("Admin", "true");
                })
                .AddPolicy("AdminOrDriver", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Driver" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrDoctor", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Doctor" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrOperator", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Operator" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrDispatcher", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Dispatcher" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                })
                .AddPolicy("AdminOrMechanic", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Mechanic" && c.Value == "true") ||
                        context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true"));
                });
        }
    }
}
