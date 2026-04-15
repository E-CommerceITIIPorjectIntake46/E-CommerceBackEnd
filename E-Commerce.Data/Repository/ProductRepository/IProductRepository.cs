using E_Commerce.Common;

namespace E_Commerce.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product>? GetProductWithCategoryAsync(int id);
        Task<PagedResult<Product>> GetProductsPaginationAsync(PaginationParameters? paginationParameters = null, ProductFilterParameters? filterParameters = null);
    }
}
