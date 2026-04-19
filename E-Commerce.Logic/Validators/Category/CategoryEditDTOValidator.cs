using E_Commerce.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.Logic
{
    public class CategoryEditDTOValidator : AbstractValidator<CategoryEditDTO>
    {
        public CategoryEditDTOValidator(IUnitOfWork unitOfWork)
        {

        }

    }
}
