using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepositoy
    {
        public OrderRepository(ECommerceDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId, string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }


        public async Task<IEnumerable<Order>> GetUserOrdersWithDetailsAsync(string userId) 
        {
            return await _dbContext.Orders
                                   .Where(o => o.UserId == userId)
                                   .Include(o => o.Items)
                                       .ThenInclude(oi => oi.Product)
                                   .OrderByDescending(o => o.OrderDate)
                                   .ToListAsync();
        }
    }
}
