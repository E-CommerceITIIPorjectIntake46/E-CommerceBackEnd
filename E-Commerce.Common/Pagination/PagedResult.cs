namespace E_Commerce.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetaData MetaData { get; set; }

    }
}
