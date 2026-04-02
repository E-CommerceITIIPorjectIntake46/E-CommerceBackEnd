using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class ApplicationSeeder
    {
        private readonly IEnumerable<ISeeder> _seeders;
        public ApplicationSeeder(IEnumerable<ISeeder> seeders)
        {
            _seeders = seeders;
        }
        public async Task SeedAllAsync(ECommerceDbContext dbContext, IServiceProvider service)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(dbContext, service);
            }
        }
    }
}
