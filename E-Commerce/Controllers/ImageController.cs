using E_Commerce.Common;
using E_Commerce.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;

        public ImageController(IImageManager imageManager, IWebHostEnvironment environment, IProductManager productManager, ICategoryManager categoryManager)
        {
            _imageManager = imageManager;
            _environment = environment;
            _productManager = productManager;
            _categoryManager = categoryManager;
        }

        [HttpPost]
        [Route("productImageUpload")]
        public async Task<ActionResult<GenericGeneralResult<ImageUploadResultDTO>>> UploadProductImage([FromForm] ImageUploadDTO imageUploadDTO, int ProductId)
        {
            var product = await _productManager.GetProductByIdAsync(ProductId);
            if (product is null)
            {
                return NotFound(product);
            }
            var oldImageURL = product.Data.ImageURL;

            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _environment.ContentRootPath;

            var result = await _imageManager.UploadImageAsync(imageUploadDTO, schema, host, basePath);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            product.Data.ImageURL = result.Data.imageURL;
            var productEditDTO = new ProductEditDTO
            {
                Id = product.Data.Id,
                Name = product.Data.Name,
                Description = product.Data.Description,
                Price = product.Data.Price,
                Count = product.Data.Count,
                ImageURL = product.Data.ImageURL,
                CategoryId = product.Data.CategoryId,
            };
            await _productManager.UpdateProductAsync(productEditDTO);

            if (!string.IsNullOrEmpty(oldImageURL))
            {
                var oldImagePath = Path.Combine(basePath, "Files", Path.GetFileName(oldImageURL));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("categoryImageUpload")]
        public async Task<ActionResult<GenericGeneralResult<ImageUploadResultDTO>>> UploadCategoryImage([FromForm] ImageUploadDTO imageUploadDTO, int CategoryId)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(CategoryId);
            if (category is null)
            {
                return NotFound(category);
            }
            var oldImageURL = category.Data.ImageURL;

            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _environment.ContentRootPath;

            var result = await _imageManager.UploadImageAsync(imageUploadDTO, schema, host, basePath);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            category.Data.ImageURL = result.Data.imageURL;
            var categoryEditDTO = new CategoryEditDTO
            {
                Id = category.Data.Id,
                Name = category.Data.Name,
                ImageURL = category.Data.ImageURL,
            };
            await _categoryManager.UpdateCategoryAsync(categoryEditDTO);

            if (!string.IsNullOrEmpty(oldImageURL))
            {
                var oldImagePath = Path.Combine(basePath, "Files", Path.GetFileName(oldImageURL));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            return Ok(result);
        }
    }
}
