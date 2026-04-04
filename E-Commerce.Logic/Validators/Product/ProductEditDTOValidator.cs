using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductEditDTOValidator : AbstractValidator<ProductEditDTO>
    {
        public ProductEditDTOValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(product => product.Name)
                .SetValidator(new ProductNameVlidator(unitOfWork));

            RuleFor(product => product.Description)
                .SetValidator(new ProductDescriptionValidator());

            RuleFor(product => product.Price)
                .SetValidator(new ProductPriceValidator());

            RuleFor(product => product.Count)
                .SetValidator(new ProductCountValidator());
        }
    }
}
