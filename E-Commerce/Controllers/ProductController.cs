using E_Commerce.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        public ProductController(IProductManager productManager) 
        {
            _productManager = productManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductReadDTO>>> GetAllProducts()
        {
            var products = await _productManager.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductReadDTO>> GetProduct([FromRoute] int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 404,
                    Message = $"Product with ID {id} not found."
                };
                return NotFound(errorResponse);
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ProductReadDTO>> CreateProduct([FromBody] ProductCreateDTO product)
        {
            if(!ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Invalid product data. Please check the input and try again."
                };
                return BadRequest(errorResponse);
            }
            var createdProduct = await _productManager.AddProductAsync(product);
            return Ok(createdProduct);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductEditDTO updatedProduct)
        {
            if(id != updatedProduct.Id)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Product ID in the URL does not match the ID in the request body."
                };
                return BadRequest(errorResponse);
            }

            if(updatedProduct == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Updated product data cannot be null."
                };
                return BadRequest(errorResponse);
            }

            if(!ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Invalid product data. Please check the input and try again."
                };
                return BadRequest(errorResponse);
            }

            await _productManager.UpdateProductAsync(updatedProduct);
            return Ok("Product Updated Successfully");
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 404,
                    Message = $"Product with ID {id} not found."
                };
                return NotFound(errorResponse);
            }

            await _productManager.DeleteProductAsync(id);
            return Ok("Product Deleted Successfully");
        }
    }
}
