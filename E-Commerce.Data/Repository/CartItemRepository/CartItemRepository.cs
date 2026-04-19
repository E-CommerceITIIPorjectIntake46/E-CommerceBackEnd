namespace E_Commerce.Data
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
