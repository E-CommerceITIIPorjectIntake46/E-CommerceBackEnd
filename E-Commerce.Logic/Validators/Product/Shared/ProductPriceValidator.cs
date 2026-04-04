using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductPriceValidator : AbstractValidator<decimal>
    {
        public ProductPriceValidator()
        {
            RuleFor(Price => Price)
                .GreaterThan(0)
                .WithMessage("Price cannot be 0 or less")
                .WithErrorCode("ERR-P-6");
        }
    }
}
