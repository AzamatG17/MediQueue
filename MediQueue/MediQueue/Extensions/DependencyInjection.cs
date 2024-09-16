using MediQueue.Domain.Interfaces.Auth;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using MediQueue.Infrastructure.Persistence;
using MediQueue.Infrastructure.Persistence.Repositories;
using MediQueue.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            //AddAuthorization(services);
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
            services.AddScoped<MediQueue.Domain.Interfaces.Services.IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoriesService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissonService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IQuestionnaireHistoryService, QuestionnaireHistoryService>();
            services.AddScoped<IPaymentServiceService, PaymentServiceService>();

            services.AddSingleton<IAuthorizationHandler, JwtPermissionHandler>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IQuestionnaireHistoryRepositoty, QuestionnaireHistoryRepository>();
            services.AddScoped<IPaymentServiceRepository, PaymentServiceRepository>();
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CategoryGetAll", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("CategoryGetAll")));

                options.AddPolicy("CategoryGetById", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("CategoryGetById")));

                options.AddPolicy("CategoryPost", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("CategoryPost")));

                options.AddPolicy("CategoryPut", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("CategoryPut")));

                options.AddPolicy("CategoryDelete", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("CategoryDelete")));

                options.AddPolicy("AllQuestionnaire", policy =>
                    policy.Requirements.Add(new JwtPermissionRequirement("AllQuestionnaire")));
            });
        }
    }
}
