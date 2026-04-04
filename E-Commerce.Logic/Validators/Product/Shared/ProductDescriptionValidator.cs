using E_Commerce.Data;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductDescriptionValidator : AbstractValidator<string>
    {
        public ProductDescriptionValidator()
        {
            RuleFor(Description => Description)
                .MaximumLength(255)
                .WithMessage("Description cannot be longer than 255 characters")
                .WithErrorCode("ERR-P-5");
        }
    }
}
