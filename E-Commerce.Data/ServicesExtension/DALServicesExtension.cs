using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Data
{
    public static class DALServicesExtension
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RESTfulConnectionString");
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ISeeder, CategorySeeder>();
            services.AddScoped<ISeeder, ProductSeeder>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
