using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork._CategoryRepository.GetAllAsync();
            
            var categoryReadDTOs = categories.Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Name = c.Name
            });
            return categoryReadDTOs;
        }
        public async Task<CategoryReadDTO>? GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork._CategoryRepository
                                            .GetByIdAsync(id);

            if(category == null)
            {
                return null;
            }

            var categoryReadDTO = new CategoryReadDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return categoryReadDTO;
        }

        public async Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name
            };

            _unitOfWork._CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();

            var categoryReadDTO = new CategoryReadDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return categoryReadDTO;
        }

        public async Task UpdateCategoryAsync(CategoryEditDTO updateCategoryDto)
        {
            var category = await _unitOfWork._CategoryRepository.GetByIdAsync(updateCategoryDto.Id);
            if(category is null)
            {
                return;
            }

            category.Name = updateCategoryDto.Name;
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork._CategoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                return;
            }

            _unitOfWork._CategoryRepository.Delete(category);
            await _unitOfWork.SaveAsync();
        }
    }
}
