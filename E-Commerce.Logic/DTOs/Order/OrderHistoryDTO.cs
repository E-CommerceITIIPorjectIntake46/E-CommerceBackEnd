using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class OrderHistoryDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItemDTO> orderItemDTOs { get; set; } = new List<OrderItemDTO>();
    }
}
