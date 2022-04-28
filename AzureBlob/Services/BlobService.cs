using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlob.Services
{
    public class BlobService : IBlobServices
    {
        private readonly BlobServiceClient _blobService;

        public BlobService(BlobServiceClient blobService)
        {
            _blobService = blobService;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            var bloblClient = blobContainerClient.GetBlobClient(name);
            return await bloblClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobString = new List<string>();
            await foreach (var item in blobs)
            {
                blobString.Add(item.Name);
            }
            return blobString;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            var bloblClient = blobContainerClient.GetBlobClient(name);
            return bloblClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            var bloblClient = blobContainerClient.GetBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            var result =await bloblClient.UploadAsync(file.OpenReadStream(), httpHeaders);
            if (result!=null)
            {
                return true;
            }
            return false;
        }
    }
}