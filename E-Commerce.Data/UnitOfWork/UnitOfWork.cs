namespace E_Commerce.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository _ProductRepository { get; }
        public ICategoryRepository _CategoryRepository { get; }
        private readonly ECommerceDbContext _dbContext;
        public UnitOfWork(IProductRepository productRepository,
                          ICategoryRepository categoryRepository,
                          ECommerceDbContext dbContext)
        {
            _ProductRepository = productRepository;
            _CategoryRepository = categoryRepository;
            _dbContext = dbContext;

        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
