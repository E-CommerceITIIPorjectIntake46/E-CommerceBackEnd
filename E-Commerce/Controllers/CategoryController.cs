using E_Commerce.Common;
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
        public async Task<ActionResult<GenericGeneralResult<IEnumerable<CategoryReadDTO>>>> GetAllCategories()
        {
            var result = await _categoryManager.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericGeneralResult<CategoryReadDTO>>> GetCategory([FromRoute] int id)
        {
            var result = await _categoryManager.GetCategoryByIdAsync(id);
            if(result == null)
            {
                
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GenericGeneralResult<CategoryReadDTO>>> CreateCategory([FromBody] CategoryCreateDTO category)
        {
            var result = await _categoryManager.AddCategoryAsync(category);
            if (!result.Success) 
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<ActionResult<GeneralResult>> UpdateCategory([FromRoute] int id, [FromBody] CategoryEditDTO updatedCategory)
        {
            if(id != updatedCategory.Id)
            {
                return BadRequest("ID in the URL does not match ID in the request body.");
            }

            var result = await _categoryManager.UpdateCategoryAsync(updatedCategory);

            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {

            var result = await _categoryManager.DeleteCategoryAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
