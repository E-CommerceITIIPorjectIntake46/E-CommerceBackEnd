using FluentValidation;

namespace E_Commerce.Logic
{
    public class ImageUploadDTOValidator : AbstractValidator<ImageUploadDTO>
    {
        private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        public ImageUploadDTOValidator()
        {
            RuleFor(i => i.File)
                .NotNull()
                .WithMessage("Image file is required.")
                .WithErrorCode("ERR-U-I-1");

            When(i => i.File != null, () =>
            {
                RuleFor(i => i.File.Length)
                    .GreaterThan(0)
                    .WithMessage("File must not be empty")
                    .WithErrorCode("ERR-U-I-1")
                    .WithName("FileSize")

                    .LessThanOrEqualTo(5_000_000)
                    .WithMessage("File Must be less than 5MB")
                    .WithErrorCode("ERR-U-I-1")
                    .WithName("FileSize");

                RuleFor(i => Path.GetExtension(i.File.FileName).ToLower())
                    .Must(e => AllowedExtensions.Contains(e))
                    .WithMessage("Unsupported file extension")
                    .WithErrorCode("ERR-U-I-1")
                    .WithName("FileExtension");
            });
        }
    }
}
