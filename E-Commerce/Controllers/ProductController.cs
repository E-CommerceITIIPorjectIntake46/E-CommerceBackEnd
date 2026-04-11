using E_Commerce.Common;
using E_Commerce.Data;
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
        public async Task<ActionResult<GenericGeneralResult<IEnumerable<ProductReadDTO>>>> GetAllProducts()
        {
            var products = await _productManager.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericGeneralResult<ProductReadDTO>>> GetProduct([FromRoute] int id)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(product);
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GenericGeneralResult<ProductReadDTO>>> CreateProduct([FromBody] ProductCreateDTO product)
        {
            var result = await _productManager.AddProductAsync(product);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductEditDTO updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest("id and product.id you inserted doesnt match");
            }

            if (updatedProduct is null)
            {
                return BadRequest("product cannot be null");
            }

            var result = await _productManager.UpdateProductAsync(updatedProduct);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var result = await _productManager.DeleteProductAsync(id);
            return Ok(result);
        }
    }
}
