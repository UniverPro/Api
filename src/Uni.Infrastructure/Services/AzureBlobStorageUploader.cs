using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Uni.Core.Exceptions;
using Uni.Core.Extensions;
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

        public async Task<string> UploadImageToStorageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
            )
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using (var stream = file.OpenReadStream())
            {
                string extension;
                try
                {
                    extension = ImageExtensions.AssertFileIsWebFriendlyImageAndGetExtension(stream);
                }
                catch (ArgumentException e)
                {
                    throw new UnsupportedMediaTypeException("The file has unsupported media type", e);
                }

                var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("images");

                await cloudBlobContainer.CreateIfNotExistsAsync(
                    BlobContainerPublicAccessType.Blob,
                    null,
                    null,
                    cancellationToken
                );

                var blobName = $"{Guid.NewGuid()}{extension}";
                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

                await cloudBlockBlob.UploadFromStreamAsync(
                    stream,
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
