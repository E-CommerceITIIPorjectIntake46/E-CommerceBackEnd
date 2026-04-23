namespace E_Commerce.Data
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
