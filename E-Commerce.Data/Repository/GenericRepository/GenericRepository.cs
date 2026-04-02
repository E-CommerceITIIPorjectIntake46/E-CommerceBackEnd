using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ECommerceDbContext _dbContext;
        public GenericRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T>? GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
      
    }
}
