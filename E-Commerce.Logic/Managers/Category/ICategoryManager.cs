namespace E_Commerce.Logic
{
    public interface ICategoryManager
    {
            Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync();
            Task<CategoryReadDTO>? GetCategoryByIdAsync(int id);
            Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO createCategoryDto);
            Task UpdateCategoryAsync(CategoryEditDTO updateCategoryDto);
            Task DeleteCategoryAsync(int id);
    }
}
