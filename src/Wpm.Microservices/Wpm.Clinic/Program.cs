using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Wpm.Clinic.Application;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;

namespace Wpm.Clinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache(); // DI memory cache
            builder.Services.AddScoped<ManagementService>();
            builder.Services.AddScoped<ClinicApplicationService>();
            builder.Services.AddDbContext<ClinicDbContext>(options =>
            {
                options.UseInMemoryDatabase("WpmClinic");
            });

            builder.Services.AddHttpClient<ManagementService>(client =>
            {
                var uri = builder.Configuration["Wpm:ManagementUri"];
                client.BaseAddress = new Uri(uri);
            })
            .AddResilienceHandler("management-pipeline", pipeline =>
            {
                pipeline.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>
                {
                    BackoffType = DelayBackoffType.Exponential,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                });
            });

            var app = builder.Build();

            // Seed in-memory DB
            app.EnsureClinicDbIsCreated();

            // Enable swagger always (Prod + Dev)
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
