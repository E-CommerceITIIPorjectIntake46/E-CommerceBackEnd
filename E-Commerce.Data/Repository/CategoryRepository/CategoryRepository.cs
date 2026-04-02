namespace E_Commerce.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ECommerceDbContext dbContext): base(dbContext)
        {
            
        }
    }
}
