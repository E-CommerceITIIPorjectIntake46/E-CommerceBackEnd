namespace E_Commerce.Logic
{
    public class CartReadDTO
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemReadDTO> CartItems { get; set; }
    }
}
