namespace E_Commerce.Data
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartWithItemsByUserIdAsync(string userId);
        Task<Cart> GetCartWithItemsAndProductsByUserIdAsync(string userId);
    }
}
