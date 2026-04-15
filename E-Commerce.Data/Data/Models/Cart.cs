namespace E_Commerce.Data
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
