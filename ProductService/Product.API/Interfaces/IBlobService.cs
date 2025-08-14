namespace Product.API.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file, string? prefix = null, CancellationToken ct = default);
        Task<string> UpsertAsync(IFormFile file, string blobName, CancellationToken ct = default); // overwrite
        Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
        Task<bool> DeleteAsync(string alt, CancellationToken ct = default);
        Task<IReadOnlyList<string>> ListAsync(string? prefix = null, CancellationToken ct = default);
        Uri GetPublicUri(string blobName);                       // for public containers
        Uri GetReadSasUri(string blobName, TimeSpan ttl);        // for private containers
    }
}
