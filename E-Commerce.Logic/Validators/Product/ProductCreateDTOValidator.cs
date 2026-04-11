using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateDTOValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(product => product.Name)
                .SetValidator(new ProductNameValidator(unitOfWork));

            RuleFor(product => product.Description)
                .SetValidator(new ProductDescriptionValidator());

            RuleFor(product => product.Price)
                .SetValidator(new ProductPriceValidator());

            RuleFor(product => product.Count)
                .SetValidator(new ProductCountValidator());
        }
    }
}
