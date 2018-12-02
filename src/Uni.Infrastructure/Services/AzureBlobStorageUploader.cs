using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.Services
{
    public class AzureBlobStorageUploader : IBlobStorageUploader
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public AzureBlobStorageUploader()
        {
            var storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
            _cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
        }

        public async Task<string> UploadImageToStorageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");

            await cloudBlobContainer.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob,
                null,
                null,
                cancellationToken
            );

            var extension = Path.GetExtension(file.FileName);
            var blobName = $"{Guid.NewGuid()}{extension}";
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            using (var fileStream = file.OpenReadStream())
            {
                await cloudBlockBlob.UploadFromStreamAsync(
                    fileStream,
                    null,
                    null,
                    null,
                    cancellationToken
                );

                return blobName;
            }
        }
    }
}
