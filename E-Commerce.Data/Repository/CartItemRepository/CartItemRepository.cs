using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ECommerceDbContext context) : base(context)
        {
        }

        public async Task<List<CartItem>> GetCartItemsOfCartWithProductAsync(int cartId)
        {
            return await _dbContext.CartItems
                                   .Include(ci => ci.Product)
                                   .Where(ci => ci.CartId == cartId)
                                   .ToListAsync();
        }
    }
}
