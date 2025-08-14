using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Product.API.Interfaces;

namespace Product.API.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _container;

        public BlobService(BlobServiceClient serviceClient, string containerName)
        {
            _container = serviceClient.GetBlobContainerClient(containerName);
            _container.CreateIfNotExists(PublicAccessType.None); // keep private by default
        }
        public async Task<string> UploadAsync(IFormFile file, string? prefix = null, CancellationToken ct = default)
        {
            var safeName = Path.GetFileName(file.FileName);
            var blobName = $"{(string.IsNullOrWhiteSpace(prefix) ? "" : prefix.TrimEnd('/') + "/")}{Guid.NewGuid():N}-{safeName}";
            var blob = _container.GetBlobClient(blobName);

            var headers = new BlobHttpHeaders { ContentType = file.ContentType };
            await using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = headers }, ct);

            return blobName; // store blobName in DB; compute URL when needed
        }

        public async Task<string> UpsertAsync(IFormFile file, string blobName, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(blobName);
            var headers = new BlobHttpHeaders { ContentType = file.ContentType };
            await using var stream = file.OpenReadStream();

            // Overwrite existing content
            await blob.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = headers, TransferOptions = new() }, ct);
            return blobName;
        }

        public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
        {
            var blob = _container.GetBlobClient(blobName);
            var resp = await blob.DownloadContentAsync(ct);
            return new MemoryStream(resp.Value.Content.ToArray()) { Position = 0 };
        }

        public async Task<bool> DeleteAsync(string alt, CancellationToken ct = default)
        {
            
            Console.WriteLine($"{alt}\n");
            var blob = _container.GetBlobClient(alt);
            var res = await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, conditions: null, ct);
            return res.Value;
        }

        public async Task<IReadOnlyList<string>> ListAsync(string? prefix = null, CancellationToken ct = default)
        {
            var names = new List<string>();
            await foreach (var item in _container.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
                names.Add(item.Name);
            return names;
        }

        public Uri GetPublicUri(string blobName) => _container.GetBlobClient(blobName).Uri;

        public Uri GetReadSasUri(string blobName, TimeSpan ttl)
        {
            var blob = _container.GetBlobClient(blobName);
            if (!blob.CanGenerateSasUri)
                throw new InvalidOperationException("Client cannot generate SAS. Use a key credential or user delegation.");

            var sas = new BlobSasBuilder
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(ttl),
            };
            sas.SetPermissions(BlobSasPermissions.Read);

            return blob.GenerateSasUri(sas);
        }

    }
}
