using E_Commerce.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Logic
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterDTOValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .WithErrorCode("ERR-U-1")
                
                .MinimumLength(3)
                .WithMessage("First name must be at least 3 characters long.")
                .WithErrorCode("ERR-U-2")
                 
                .MaximumLength(100)
                .WithMessage("First name must not exceed 100 characters.")
                .WithErrorCode("ERR-U-3");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .WithErrorCode("ERR-U-4")
                
                .MinimumLength(3)
                .WithMessage("Last name must be at least 3 characters long.")
                .WithErrorCode("ERR-U-5")
                 
                .MaximumLength(100)
                .WithMessage("Last name must not exceed 100 characters.")
                .WithErrorCode("ERR-U-6");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .WithErrorCode("ERR-U-7")

                .MaximumLength(250)
                .WithMessage("Email must not exceed 250 characters.")
                .WithErrorCode("ERR-U-8")

                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .WithErrorCode("ERR-U-9");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username is required.")
                .WithErrorCode("ERR-U-10")

                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters long.")
                .WithErrorCode("ERR-U-11")

                .MaximumLength(100)
                .WithMessage("Username must not exceed 100 characters.")
                .WithErrorCode("ERR-U-12")
                
                .MustAsync(CheckUserNameUnique)
                .WithMessage("Username is already taken.")
                .WithErrorCode("ERR-U-13");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .WithErrorCode("ERR-U-14")

                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .WithErrorCode("ERR-U-15")

                .MaximumLength(100)
                .WithMessage("Password must not exceed 100 characters.")
                .WithErrorCode("ERR-U-16")

                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")
                .WithErrorCode("ERR-U-17");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Confirm password must match the password.")
                .WithErrorCode("ERR-U-18");
        }
        private async Task<bool> CheckUserNameUnique(string userName, CancellationToken cancellationToken)
        {
            var ISUnique = !await _userManager.Users
                                             .AnyAsync(u => u.UserName == userName);
            return ISUnique;
        }
    }
}
