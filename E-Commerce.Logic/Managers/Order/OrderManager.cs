using E_Commerce.Common;
using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericGeneralResult<OrderDetailsDTO>> GetOrderDetailsAsync(int orderId, string userId)
        {
            var order = await _unitOfWork._OrderRepository
                .GetOrderWithDetailsAsync(orderId, userId);

            if (order == null)
            {
                return GenericGeneralResult<OrderDetailsDTO>.NotFound("Order not found or does not belong to the user.");
            }

            var orderDetails = new OrderDetailsDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = order.Items.Select(i => new OrderItemDetailsDTO
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return GenericGeneralResult<OrderDetailsDTO>.SuccessResult(orderDetails, "Order details retrieved successfully.");
        }

        public async Task<GenericGeneralResult<IEnumerable<OrderHistoryDTO>>> GetOrderHistoryAsync(string userId)
        {
            var orders = await _unitOfWork._OrderRepository.GetUserOrdersWithDetailsAsync(userId);

            if (!orders.Any())
            {
                return GenericGeneralResult<IEnumerable<OrderHistoryDTO>>.NotFound("No orders found for the user.");
            }

            var orderHistory = orders.Select(o => new OrderHistoryDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                orderItemDTOs = o.Items.Select(i => new OrderItemDTO
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            });

            return GenericGeneralResult<IEnumerable<OrderHistoryDTO>>.SuccessResult(orderHistory, "Order history retrieved successfully.");
        }

        public async Task<GeneralResult> PlaceOrderAsync(OrderRequest orderRequest)
        {
            var carts = await _unitOfWork._CartRepository.GetCartWithItemsAndProductsByUserIdAsync(orderRequest.UserId);

            if (carts == null || !carts.CartItems.Any())
            {
                return GeneralResult.NotFound("No cart found for the user.");
            }

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in carts.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price
                };

                totalAmount += orderItem.UnitPrice * orderItem.Quantity;
                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                UserId = orderRequest.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.pending,
                Items = orderItems
            };

            _unitOfWork._OrderRepository.Add(order);
            _unitOfWork._CartRepository.Delete(carts);
            await _unitOfWork.SaveAsync();

            return GeneralResult.SuccessResult("Order placed successfully.");
        }
    }
}
