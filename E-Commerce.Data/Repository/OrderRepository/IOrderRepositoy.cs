namespace E_Commerce.Data
{
    public interface IOrderRepositoy : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int orderId, string userId);
        Task<IEnumerable<Order>> GetUserOrdersWithDetailsAsync(string userId);
    }
}
