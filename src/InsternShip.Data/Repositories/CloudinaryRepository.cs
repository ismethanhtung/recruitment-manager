using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InsternShip.Data.Repositories
{
    public class CloudinaryRepository: ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryRepository(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        [Obsolete]
        public async Task<UploadFileModel> UploadFile(IFormFile? file)
        {

            if (file == null || file.Length > 0)
            {
                try
                {
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        AccessMode = "public",
                        //UseFilename = true,
                        UniqueFilename = false,
                        Folder = "CVFiles"
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    var publicUrl = _cloudinary.Api.UrlImgUp.Transform(new Transformation()).BuildUrl(uploadResult.PublicId + "." + uploadResult.Format);
                    var publicId = uploadResult.PublicId;
                    var infoFile = new UploadFileModel
                    {
                        UrlFile = publicUrl,
                        PublicIdFile = publicId
                    };
                    return infoFile;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new MissingFieldException(MissingFieldMessage.MissingFile);
            }

        }

        public async Task<bool> RemoveFile(string? publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                throw new MissingFieldException(MissingFieldMessage.MissingPublicId);
            }
            else
            {
                try
                {
                    DeletionParams deletionParams = new DeletionParams(publicId);
                    DeletionResult result = await _cloudinary.DestroyAsync(deletionParams);
                    return result.Result == "ok";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }
    }
}
