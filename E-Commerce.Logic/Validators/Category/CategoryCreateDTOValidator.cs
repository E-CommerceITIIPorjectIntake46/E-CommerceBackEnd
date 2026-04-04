using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryCreateDTOValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(category => category.Name)
                .SetValidator(new CategoryNameValidator(unitOfWork));
        }
    }
}
