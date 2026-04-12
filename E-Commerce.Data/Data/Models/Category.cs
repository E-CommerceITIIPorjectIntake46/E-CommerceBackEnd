namespace E_Commerce.Data
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageURL { get; set; }

        //-----------------------------------------------------------------------------//
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
        //-----------------------------------------------------------------------------//
    }
}
