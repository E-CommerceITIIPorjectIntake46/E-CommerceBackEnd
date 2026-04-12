using E_Commerce.Common;

namespace E_Commerce.Logic
{
    public interface ICategoryManager
    {
            Task<GenericGeneralResult<IEnumerable<CategoryReadDTO>>> GetAllCategoriesAsync();
            Task<GenericGeneralResult<CategoryReadDTO>>? GetCategoryByIdAsync(int id);
            Task<GenericGeneralResult<CategoryReadDTO>> AddCategoryAsync(CategoryCreateDTO createCategoryDto);
            Task<GeneralResult> UpdateCategoryAsync(CategoryEditDTO updateCategoryDto);
            Task<GeneralResult> DeleteCategoryAsync(int id);
    }
}
