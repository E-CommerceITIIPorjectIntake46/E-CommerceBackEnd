namespace E_Commerce.Data
{
    public interface ISeeder
    {
        Task SeedAsync(ECommerceDbContext dbContext, IServiceProvider service);
    }
}
