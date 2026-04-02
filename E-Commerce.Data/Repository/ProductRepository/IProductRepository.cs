namespace E_Commerce.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product>? GetProductWithCategoryAsync(int id);
    }
}
