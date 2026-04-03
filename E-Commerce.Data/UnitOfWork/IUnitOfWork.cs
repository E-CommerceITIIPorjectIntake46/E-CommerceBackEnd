namespace E_Commerce.Data
{
    public interface IUnitOfWork
    {
        public IProductRepository _ProductRepository { get; }
        public ICategoryRepository _CategoryRepository { get; }
        Task<int> SaveAsync();
    }
}
