using E_Commerce.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryReadDTO>>> GetAllCategories()
        {
            var categories = await _categoryManager.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryReadDTO>> GetCategory(int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if(category == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 404,
                    Message = $"Category with ID {id} not found."
                };
                return NotFound(errorResponse);
            }
            return Ok(category);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<CategoryReadDTO>> CreateCategory([FromBody] CategoryCreateDTO category)
        {
            if (!ModelState.IsValid) 
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Invalid category data. Please check the input and try again."
                };
                return BadRequest(errorResponse);
            }
            var createdCategory = await _categoryManager.CreateCategoryAsync(category);
            return Ok(createdCategory);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryEditDTO updatedCategory)
        {
            if(id != updatedCategory.Id)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "ID in the URL does not match ID in the request body."
                };
                return BadRequest(errorResponse);
            }

            if(updatedCategory == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Updated category data cannot be null."
                };
                return BadRequest(errorResponse);
            }

            if (!ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = 400,
                    Message = "Invalid category data. Please check the input and try again."
                };
                return BadRequest(errorResponse);
            }

            await _categoryManager.UpdateCategoryAsync(updatedCategory);
            return Ok("Category Updated Sucessfully");
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            var category = await _categoryManager.GetCategoryByIdAsync(id);
            if(category == null)
            {
                var errorResponse = new
                {
                    ErrorCode = 404,
                    Message = $"Category with ID {id} not found."
                };
                return NotFound(errorResponse);
            }

            await _categoryManager.DeleteCategoryAsync(id);
            return Ok("Category Deleted Sucessfully");
        }
    }
}
