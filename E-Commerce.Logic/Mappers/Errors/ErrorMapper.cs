using E_Commerce.Common;
using FluentValidation.Results;

namespace E_Commerce.Logic
{
    public class ErrorMapper : IErrorMapper
    {
        public Dictionary<string, List<Error>> MapError(ValidationResult validationResult)
        {
            return validationResult.Errors
                                   .GroupBy(e => e.PropertyName)
                                   .ToDictionary(
                                       g => g.Key,
                                       g => g.Select(e => new Error
                                       {
                                           Code = e.ErrorCode,
                                           Message = e.ErrorMessage
                                       }).ToList()
                                   );
        }
    }
}
