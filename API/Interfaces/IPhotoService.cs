using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        // ImageUploadResult from Cloudinary Actions and A IFormFile from aspnetcore http
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        // Each file uploaded will have a publicId and we need it to delete
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}