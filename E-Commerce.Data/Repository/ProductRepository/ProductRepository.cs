using E_Commerce.Common;
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

        public async Task<PagedResult<Product>> GetProductsPaginationAsync(
            PaginationParameters? paginationParameters = null,
            ProductFilterParameters? filterParameters = null)
        {
            IQueryable<Product> query = _dbContext.Set<Product>().AsQueryable();
            query.Include(p => p.Category);

            if(filterParameters != null)
            {
                query = ApplyFilters(query, filterParameters);
            }

            var totalCount = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1,50);

            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PagedResult<Product>()
            {
                Items = items,
                MetaData = new PaginationMetaData
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasNext = pageNumber < totalPages,
                    HasPrevious = pageNumber > totalPages,
                }
            };
        }
        private IQueryable<Product> ApplyFilters(IQueryable<Product> query,ProductFilterParameters? productFilterParameters)
        {
            if (productFilterParameters.MinCount > 0)
            {
                query = query.Where(p => p.Count >= productFilterParameters.MinCount);
            }
            if (productFilterParameters.MaxCount > 0)
            {
                query = query.Where(p => p.Count <= productFilterParameters.MaxCount);
            }
            if (productFilterParameters.MinPrice > 0)
            {
                query =  query.Where(p => p.Price >= productFilterParameters.MinPrice);
            }
            if (productFilterParameters.MaxPrice > 0)
            {
                query = query.Where(p => p.Price <= productFilterParameters.MaxPrice);
            }

            if (productFilterParameters.Search is not null)
            {
                query = query.Where(p => p.Name.Contains(productFilterParameters.Search) || p.Description.Contains(productFilterParameters.Search));
            }
            return query;
        }
    }
}
