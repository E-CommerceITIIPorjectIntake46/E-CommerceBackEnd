using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class CategoryNameValidator : AbstractValidator<string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryNameValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(Name => Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .WithErrorCode("ERR-C-001")

                .MinimumLength(3)
                .WithMessage("Category name must be at least 3 characters long.")
                .WithErrorCode("ERR-C-002")

                .MaximumLength(50)
                .WithMessage("Category name must not exceed 50 characters.")
                .WithErrorCode("ERR-C-003")

                .MustAsync(CheckNameUnique)
                .WithMessage("Category name must be unique.")
                .WithErrorCode("ERR-C-004");
        }

        private async Task<bool> CheckNameUnique(string name, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork._CategoryRepository.GetAllAsync();

            var isUnique = !category.Any(c => c.Name == name);
            return isUnique;
        }
    }
}
