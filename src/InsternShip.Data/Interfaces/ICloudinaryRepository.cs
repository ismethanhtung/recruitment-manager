using InsternShip.Data.Model;
using Microsoft.AspNetCore.Http;

namespace InsternShip.Data.Interfaces
{
    public interface ICloudinaryRepository
    {
        public Task<UploadFileModel> UploadFile(IFormFile? file);
        public Task<bool> RemoveFile(string? publicId);
    }
}
