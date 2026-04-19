using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ECommerceDbContext context) : base(context)
        {
        }

        public async Task<Cart> GetCartWithItemsByUserIdAsync(int userId)
        {
            return await _dbContext.Carts
                                   .Include(c => c.CartItems)
                                   .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
