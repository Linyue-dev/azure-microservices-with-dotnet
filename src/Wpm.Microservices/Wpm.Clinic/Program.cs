
using Microsoft.EntityFrameworkCore;
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
            builder.Services.AddScoped<ManagementService>();
            builder.Services.AddScoped<ClinicApplicationService>();
            builder.Services.AddDbContext<ClinicDbContext>(options =>
            {
                options.UseInMemoryDatabase("WpmClinic");
            });

            builder.Services.AddHttpClient<ManagementService>(client =>
            {
                var uri = builder.Configuration.GetValue<string>("Wpm:ManagementUri");
                client.BaseAddress = new Uri(uri);
            });

            var app = builder.Build();

            // Seed in-memory DB
            app.EnsureClinicDbIsCreated();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
