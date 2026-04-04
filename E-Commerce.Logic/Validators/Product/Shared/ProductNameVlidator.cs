using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductNameVlidator : AbstractValidator<string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductNameVlidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(Name => Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty")
                .WithErrorCode("ERR-P-1")

                .MinimumLength(3)
                .WithMessage("Name at least 3 char")
                .WithErrorCode("ERR-P-2")

                .MaximumLength(50)
                .WithMessage("Name cannot be longer than 50 chars")
                .WithErrorCode("ERR-P-3")

                .MustAsync(CheckNameUnique)
                .WithMessage("Name should be unique")
                .WithErrorCode("ERR-P-4");

        }

        private async Task<bool> CheckNameUnique(string name, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork._ProductRepository.GetAllAsync();

            var isUnique = products.Any(p => p.Name == name);
            return isUnique;
        }
    }
}
