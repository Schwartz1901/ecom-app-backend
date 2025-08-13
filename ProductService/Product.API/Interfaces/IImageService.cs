namespace Product.API.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
