namespace E_Commerce.Common
{
    public class ProductFilterParameters : BaseFilterParameters
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
