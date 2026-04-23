using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDetailsDTO> Items { get; set; } = new();
    }

    public class OrderItemDetailsDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
