namespace E_Commerce.Logic
{
    public class ProductEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public int? CategoryId { get; set; }
    }
}
