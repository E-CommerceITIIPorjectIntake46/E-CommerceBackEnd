using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Logic
{
    public static class BLLServicesExtension
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ApplicationSeeder>();
        }
    }
}
