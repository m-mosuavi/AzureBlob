using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlob.Services
{
    public class ContainerService : IContainerServices
    {
        private readonly BlobServiceClient _blobService;

        public ContainerService(BlobServiceClient blobService)
        {
            _blobService = blobService;
        }

        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerAndBlobNames = new();
            containerAndBlobNames.Add("Account Name : " + _blobService.AccountName);
            containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
            await foreach (BlobContainerItem blobContainerItem in _blobService.GetBlobContainersAsync())
            {
                containerAndBlobNames.Add("--" + blobContainerItem.Name);
                BlobContainerClient _blobContainer =
                      _blobService.GetBlobContainerClient(blobContainerItem.Name);
                await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    //get metadata
                    var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string blobToAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                    }

                    containerAndBlobNames.Add("------" + blobToAdd);
                }
                containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");

            }
            return containerAndBlobNames;
        }

        public async Task<List<string>> GetAllContainers()
        {
            List<string> containerName = new();
            await foreach (BlobContainerItem item in _blobService.GetBlobContainersAsync())
            {
                containerName.Add(item.Name);
            }
            return containerName;
        }
    }
}
