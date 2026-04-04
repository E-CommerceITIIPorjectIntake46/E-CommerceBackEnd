using FluentValidation;

namespace E_Commerce.Logic
{
    public class ProductCountValidator : AbstractValidator<int>
    {
        public ProductCountValidator()
        {
            RuleFor(Count => Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("count cannot nagtive")
                .WithErrorCode("ERR-P-7");
        }
    }
}
