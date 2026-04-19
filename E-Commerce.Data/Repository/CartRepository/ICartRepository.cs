namespace E_Commerce.Data
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartWithItemsByUserIdAsync(int userId);
    }
}
