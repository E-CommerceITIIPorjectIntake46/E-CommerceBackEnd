using E_Commerce.Common;
using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public interface IProductManager
    {
        Task<GenericGeneralResult<IEnumerable<ProductReadDTO>>> GetAllProductsAsync();
        Task<GenericGeneralResult<ProductReadDTO>> GetProductByIdAsync(int id);
        Task<GenericGeneralResult<ProductReadDTO>> AddProductAsync(ProductCreateDTO product);      
        Task<GeneralResult> UpdateProductAsync(ProductEditDTO productToUpdate);
        Task<GeneralResult> DeleteProductAsync(int id);
    }
}
