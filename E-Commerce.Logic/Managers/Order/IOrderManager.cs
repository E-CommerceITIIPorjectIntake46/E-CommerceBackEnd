using E_Commerce.Common;

namespace E_Commerce.Logic
{
    public interface IOrderManager
    {
        Task<GenericGeneralResult<OrderDetailsDTO>> GetOrderDetailsAsync(int orderId, string userId);
        Task<GenericGeneralResult<IEnumerable<OrderHistoryDTO>>> GetOrderHistoryAsync(string userId);
        Task<GeneralResult> PlaceOrderAsync(OrderRequest orderRequest);
    }
}
