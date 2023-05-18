using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); //imageUploadRes va a dar un publicId que se storea en db
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}