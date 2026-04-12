using E_Commerce.Common;
using FluentValidation;

namespace E_Commerce.Logic
{
    public class ImageManager : IImageManager
    {
        private readonly IValidator<ImageUploadDTO> _imageUploadValidator;
        private readonly IErrorMapper _errorMapper;

        public ImageManager(IValidator<ImageUploadDTO> imageUploadValidator, IErrorMapper errorMapper)
        {
            _imageUploadValidator = imageUploadValidator;
            _errorMapper = errorMapper;
        }
        public async Task<GenericGeneralResult<ImageUploadResultDTO>> UploadImageAsync(ImageUploadDTO imageUploadDTO, string basePath, string? schema, string? host)
        {
            if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(host))
            {
                return GenericGeneralResult<ImageUploadResultDTO>.FailResult("Schema and host must be provided.");
            }

            var result = await _imageUploadValidator.ValidateAsync(imageUploadDTO);
            if (!result.IsValid)
            {
                var errors = _errorMapper.MapError(result);
                return GenericGeneralResult<ImageUploadResultDTO>.FailResult(errors);
            }

            var file = imageUploadDTO.File;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var cleanName = Path.GetFileNameWithoutExtension(file.FileName).Trim().Replace(" ", "-");
            var uniqueName = $"{cleanName}-{Guid.NewGuid()}{extension}";
            var directoryPath = Path.Combine(basePath, "Files");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var url = $"{schema}://{host}/Files/{uniqueName}";
            var imageUploadResultDTO = new ImageUploadResultDTO(url);
            return GenericGeneralResult<ImageUploadResultDTO>.SuccessResult(imageUploadResultDTO);
        }
    }
}
