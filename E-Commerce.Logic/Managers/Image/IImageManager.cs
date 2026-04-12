using E_Commerce.Common;

namespace E_Commerce.Logic
{
    public interface IImageManager
    {
        Task<GenericGeneralResult<ImageUploadResultDTO>> UploadImageAsync(ImageUploadDTO imageUploadDTO, string basePath, string? schema, string? host);
    }
}
