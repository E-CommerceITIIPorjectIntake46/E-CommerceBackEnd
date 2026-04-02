namespace E_Commerce.Data
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        //-----------------------------------------------------------------------------//
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        //-----------------------------------------------------------------------------//
    }
}
