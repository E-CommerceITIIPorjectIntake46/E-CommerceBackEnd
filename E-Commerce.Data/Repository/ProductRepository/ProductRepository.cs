using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ECommerceDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _dbContext.Products
                                   .Include(p => p.Category)
                                   .ToListAsync();
        }

        public Task<Product>? GetProductWithCategoryAsync(int id)
        {
            return _dbContext.Products
                             .Include(p => p.Category)
                             .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
