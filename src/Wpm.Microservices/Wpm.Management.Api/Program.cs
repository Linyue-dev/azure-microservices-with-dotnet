
using Microsoft.EntityFrameworkCore;
using System;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api
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

            // Configure EF Core to use an InMemory database.
            // This is used for development/demo purposes and does not require a real DB server.
            builder.Services.AddDbContext<ManagementDbContext>( options =>
            {
                options.UseInMemoryDatabase("WpmManagement");
            });

            var app = builder.Build();
            app.EnsureDbIsCreated();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
