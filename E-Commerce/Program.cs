using E_Commerce.Data;
using E_Commerce.Logic;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDALServices(builder.Configuration);
            builder.Services.AddBLLServices();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
                db.Database.Migrate();

                var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
                await seeder.SeedAllAsync(db, scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
