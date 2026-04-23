namespace E_Commerce.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository _ProductRepository { get; }
        public ICategoryRepository _CategoryRepository { get; }
        public ICartRepository _CartRepository {  get; }
        public ICartItemRepository _CartItemRepository { get; }
        public IOrderRepositoy _OrderRepository { get; }
        public IOrderItemRepository _OrderItemRepository { get; }

        private readonly ECommerceDbContext _dbContext;
        public UnitOfWork(IProductRepository productRepository,
                          ICategoryRepository categoryRepository,
                          ICartRepository cartRepository,
                          ICartItemRepository cartItemRepository,
                          IOrderRepositoy orderRepository,
                          IOrderItemRepository orderItemRepository,
                          ECommerceDbContext dbContext)
        {
            _ProductRepository = productRepository;
            _CategoryRepository = categoryRepository;
            _CartRepository = cartRepository;
            _CartItemRepository = cartItemRepository;
            _OrderRepository = orderRepository;
            _OrderItemRepository = orderItemRepository;
            _dbContext = dbContext;

        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
