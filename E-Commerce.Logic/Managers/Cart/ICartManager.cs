using E_Commerce.Common;

namespace E_Commerce.Logic
{
    public interface ICartManager
    {
        Task<GenericGeneralResult<CartReadDTO>> GetCartByUserIdAsync(string userId);
        Task<GeneralResult> AddToCartAsync(string userId, int productId, int quantity);
        Task<GeneralResult> RemoveFromCartAsync(string userId, int cartItemId);
        Task<GeneralResult> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity);
    }
}
