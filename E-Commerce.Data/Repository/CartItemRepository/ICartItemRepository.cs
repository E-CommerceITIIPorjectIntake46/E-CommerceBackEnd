namespace E_Commerce.Data
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<List<CartItem>> GetCartItemsOfCartWithProductAsync(int cartId);
    }
}
