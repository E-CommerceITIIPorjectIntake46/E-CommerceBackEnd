using E_Commerce.Common;
using FluentValidation.Results;

namespace E_Commerce.Logic
{
    public interface IErrorMapper
    {
        Dictionary<string, List<Error>> MapError(ValidationResult validationResult);
    }
}
