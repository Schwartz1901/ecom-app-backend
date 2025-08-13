using Azure.Storage.Blobs;
using Product.API.Interfaces;

namespace Product.API.Services
{
    public class ImageService : IImageService
    {
        private readonly BlobContainerClient _container;
        public ImageService(BlobContainerClient container) => _container = container;

        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);

            var headers = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = file.ContentType // "image/jpeg", "image/png", etc.
            };

            await blobClient.UploadAsync(file.OpenReadStream(), new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = headers
            });

            return "Upload successfully";
        }
    }
}
