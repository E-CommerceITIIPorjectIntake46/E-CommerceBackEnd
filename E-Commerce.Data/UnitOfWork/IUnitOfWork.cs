namespace E_Commerce.Data
{
    public interface IUnitOfWork
    {
        public IProductRepository _ProductRepository { get; }
        public ICategoryRepository _CategoryRepository { get; }
        public ICartRepository _CartRepository { get; }
        public ICartItemRepository _CartItemRepository { get; }
        public IOrderRepositoy _OrderRepository { get; }
        public IOrderItemRepository _OrderItemRepository { get; }
        Task<int> SaveAsync();
    }
}
