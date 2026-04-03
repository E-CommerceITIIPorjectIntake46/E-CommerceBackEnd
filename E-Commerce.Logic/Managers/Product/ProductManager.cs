using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork._ProductRepository
                                            .GetProductsWithCategoryAsync();

            var productsReadDTO = products.Select(p => new ProductReadDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Count = p.Count,
                CategoryId = p.CategoryId,
                Category = p.Category != null ? p.Category.Name : string.Empty
            });
            return productsReadDTO;
        }

        public async Task<ProductReadDTO> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork._ProductRepository
                                           .GetProductWithCategoryAsync(id);

            if (product == null)
            {
                return null;
            }

            var productReadDTO = new ProductReadDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Count = product.Count,
                CategoryId = product.CategoryId,
                Category = product.Category != null ? product.Category.Name : string.Empty
            };
            return productReadDTO;
        }

        public async Task<ProductReadDTO> AddProductAsync(ProductCreateDTO product)
        {
            var productToAdd = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Count = product.Count,
                CategoryId = product.CategoryId
            };

            _unitOfWork._ProductRepository.Add(productToAdd);
            await _unitOfWork.SaveAsync();

            var productReadDTO = new ProductReadDTO
            {
                Name = productToAdd.Name,
                Description = productToAdd.Description,
                Price = productToAdd.Price,
                Count = productToAdd.Count,
                CategoryId = productToAdd.CategoryId,
                Category = productToAdd.Category != null ? productToAdd.Category.Name : string.Empty
            };
            return productReadDTO;
        }
        public async Task UpdateProductAsync(ProductEditDTO productToUpdate)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(productToUpdate.Id);
            if (product == null)
            {
                return;
            }

            product.Name = productToUpdate.Name;
            product.Description = productToUpdate.Description;
            product.Price = productToUpdate.Price;
            product.Count = productToUpdate.Count;
            product.CategoryId = productToUpdate.CategoryId;
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(id);
            if(product == null)
            {
                return;
            }

            _unitOfWork._ProductRepository.Delete(product);
            await _unitOfWork.SaveAsync();
        }
    }
}
