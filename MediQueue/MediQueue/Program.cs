using MediQueue.Extensions;
using MediQueue.Helpers;
using Serilog;

namespace MediQueue;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                      .WithHeaders("X-Requested-With", "Content-Type", "Origin", "Accept");
            });
        });

        //Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Verbose()
        //    .Enrich.FromLogContext()
        //    .WriteTo.Console(new CustomJsonFormatter())
        //    .WriteTo.File(new CustomJsonFormatter(), "logs/logs.txt", rollingInterval: RollingInterval.Day)
        //    .WriteTo.File(new CustomJsonFormatter(), "logs/error_.txt", Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
        //    .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //DependencyInjection.ConfigureServices(builder.Services, configuration);
        builder.Services
            .AddConfigurationOptions(configuration)
            .ConfigureServices(configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseErrorHandler();
        //app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseMiddleware<TokenValidationMiddleware>();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
