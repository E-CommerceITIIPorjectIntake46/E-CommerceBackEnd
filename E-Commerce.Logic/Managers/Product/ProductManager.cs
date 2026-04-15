using E_Commerce.Common;
using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductCreateDTO> _productCreateValidator;
        private readonly IValidator<ProductEditDTO> _productEditValidator;
        private readonly IErrorMapper _errorMapper;
        public ProductManager(IUnitOfWork unitOfWork,
                              IValidator<ProductCreateDTO> productCreateValidator,
                              IValidator<ProductEditDTO> productEditValidator,
                              IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _productCreateValidator = productCreateValidator;
            _productEditValidator = productEditValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GenericGeneralResult<IEnumerable<ProductReadDTO>>> GetAllProductsAsync()
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
            return GenericGeneralResult<IEnumerable<ProductReadDTO>>.SuccessResult(productsReadDTO, "Products retrieved successfully");
        }

        public async Task<GenericGeneralResult<ProductReadDTO>> GetProductByIdAsync(int id)
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
            return GenericGeneralResult<ProductReadDTO>.SuccessResult(productReadDTO, "Product retrieved successfully");
        }

        public async Task<GenericGeneralResult<ProductReadDTO>> AddProductAsync(ProductCreateDTO product)
        {
            var validationResult = await _productCreateValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GenericGeneralResult<ProductReadDTO>.FailResult(errors, "Validation failed");
            }

            if(product.CategoryId is not null)
            {
                var categoryExists = await _unitOfWork._CategoryRepository.GetByIdAsync(product.CategoryId ?? 0);
                if (categoryExists == null)
                {
                    return GenericGeneralResult<ProductReadDTO>.FailResult("Category not found");
                }
            }

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
            return GenericGeneralResult<ProductReadDTO>.SuccessResult(productReadDTO, "Product added successfully");
        }
        public async Task<GeneralResult> UpdateProductAsync(ProductEditDTO productToUpdate)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(productToUpdate.Id);
            if (product == null)
            {
                return GeneralResult.NotFound($"Product with ID {productToUpdate.Id} not found.");
            }

            var validationResult = await _productEditValidator.ValidateAsync(productToUpdate);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult.FailResult(errors, "Validation failed");
            }

            if(productToUpdate.CategoryId is not null)
            {
                var categoryExists = await _unitOfWork._CategoryRepository.GetByIdAsync(productToUpdate.CategoryId ?? 0);
                if (categoryExists == null)
                {
                    return GeneralResult.FailResult("Category not found");
                }
            }

            product.Name = productToUpdate.Name;
            product.Description = productToUpdate.Description;
            product.Price = productToUpdate.Price;
            product.Count = productToUpdate.Count;
            product.CategoryId = productToUpdate.CategoryId;
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Product updated successfully");
        }
        public async Task<GeneralResult> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetByIdAsync(id);
            if(product == null)
            {
                return GeneralResult.NotFound($"Product with ID {id} not found.");
            }

            _unitOfWork._ProductRepository.Delete(product);
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Product deleted successfully");
        }
        public async Task<GenericGeneralResult<PagedResult<Product>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters)
        {
            var pagedProducts = await _unitOfWork._ProductRepository.GetProductsPaginationAsync(paginationParameters, productFilterParameters);

            return GenericGeneralResult<PagedResult<Product>>.SuccessResult(pagedProducts, "Products retrieved successfully");
        }
    }
}
