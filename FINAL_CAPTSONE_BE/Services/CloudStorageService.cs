using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CloudStorageService : ICloudStorageRepository
    {
        private readonly BlobServiceClient _blobServiceClient;

        public CloudStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(string userId, Stream fileStream, string fileName, string contentType)
        {
            string containerName = "images";
            string filePath = $"{userId}/{fileName}";

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(filePath);
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

            return blobClient.Uri.ToString();
        }
    }
}
