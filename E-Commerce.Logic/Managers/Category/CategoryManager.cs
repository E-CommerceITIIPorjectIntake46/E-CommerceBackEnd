using E_Commerce.Common;
using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryCreateDTO> _categoryCreateValidator;
        private readonly IValidator<CategoryEditDTO> _categoryEditValidator;
        private readonly IErrorMapper _errorMapper;
        public CategoryManager(IUnitOfWork unitOfWork,
                               IValidator<CategoryCreateDTO> categoryCreateValidator,
                               IValidator<CategoryEditDTO> categoryEditValidator,
                               IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _categoryCreateValidator = categoryCreateValidator;
            _categoryEditValidator = categoryEditValidator;
            _errorMapper = errorMapper;
        }
        public async Task<GenericGeneralResult<IEnumerable<CategoryReadDTO>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork._CategoryRepository.GetAllAsync();
            
            var categoryReadDTOs = categories.Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Name = c.Name
            });
            return GenericGeneralResult<IEnumerable<CategoryReadDTO>>.SuccessResult(categoryReadDTOs, "Categories retreived successfully");
        }
        public async Task<GenericGeneralResult<CategoryReadDTO>>? GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork._CategoryRepository
                                            .GetByIdAsync(id);

            if(category == null)
            {
                return GenericGeneralResult<CategoryReadDTO>.NotFound($"Category with {id} not found");
            }

            var categoryReadDTO = new CategoryReadDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return GenericGeneralResult<CategoryReadDTO>.SuccessResult(categoryReadDTO);
        }

        public async Task<GenericGeneralResult<CategoryReadDTO>> AddCategoryAsync(CategoryCreateDTO createCategoryDto)
        {
            var validationResult = await _categoryCreateValidator.ValidateAsync(createCategoryDto);
            if(!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GenericGeneralResult<CategoryReadDTO>.FailResult(errors, "Validation failed");
            }

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
            return GenericGeneralResult<CategoryReadDTO>.SuccessResult(categoryReadDTO, "Category added successfully");
        }

        public async Task<GeneralResult> UpdateCategoryAsync(CategoryEditDTO updateCategoryDto)
        {
            var category = await _unitOfWork._CategoryRepository.GetByIdAsync(updateCategoryDto.Id);
            if(category is null)
            {
                return GeneralResult.NotFound($"Category with ID {updateCategoryDto.Id} not found.");
            }

            var validationResult = await _categoryEditValidator.ValidateAsync(updateCategoryDto);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult.FailResult(errors, "Validation failed");
            }

            category.Name = updateCategoryDto.Name;
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Category updated successfully");
        }

        public async Task<GeneralResult> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork._CategoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                return GeneralResult.NotFound($"Category with ID {id} not found.");
            }

            _unitOfWork._CategoryRepository.Delete(category);
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Category deleted successfully");
        }
    }
}
