using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace E_Commerce.Logic
{
    public static class BLLServicesExtension
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ApplicationSeeder>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddValidatorsFromAssembly(typeof(BLLServicesExtension).Assembly);
            services.AddScoped<IErrorMapper, ErrorMapper>();
        }
    }
}
