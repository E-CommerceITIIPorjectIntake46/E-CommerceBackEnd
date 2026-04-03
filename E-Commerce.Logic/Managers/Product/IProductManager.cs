using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public interface IProductManager
    {
        Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync();
        Task<ProductReadDTO> GetProductByIdAsync(int id);
        Task<ProductReadDTO> AddProductAsync(ProductCreateDTO product);      
        Task UpdateProductAsync(ProductEditDTO productToUpdate);
        Task DeleteProductAsync(int id);
    }
}
