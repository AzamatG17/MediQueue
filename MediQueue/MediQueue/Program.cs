
using MediQueue.Extensions;
using System.Configuration;

namespace MediQueue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddCors(options =>
             {
                 options.AddPolicy("AllowSpecificOrigins", policy =>
                 {
                     policy.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                           .WithHeaders("X-Requested-With", "Content-Type", "Origin", "Accept");
                 });
             });

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

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
