using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Uni.Api.Core.Exceptions;
using Uni.Api.Core.Extensions;
using Uni.Api.Infrastructure.Interfaces.Services;

namespace Uni.Api.Infrastructure.Services
{
    public class AzureBlobStorageUploader : IBlobStorageUploader
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public AzureBlobStorageUploader()
        {
            var storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
            var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<string> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
            )
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream, cancellationToken);

                string formatDescription;
                try
                {
                    formatDescription = ImageExtensions.AssertFileIsWebFriendlyImageAndGetFormatDescription(stream);
                }
                catch (ArgumentException e)
                {
                    throw new UnsupportedMediaTypeException("The file has unsupported media type", e);
                }

                stream.Seek(0, SeekOrigin.Begin);
                
                var blobName = $"{Guid.NewGuid()}.{formatDescription}";

                return await UploadToStorageInternal(stream, blobName, cancellationToken);
            }
        }

        private async Task<string> UploadToStorageInternal(Stream stream, [NotNull] string blobName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(blobName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(blobName));
            }

            var cloudBlobContainer = _cloudBlobClient.GetContainerReference("images");

            await cloudBlobContainer.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob,
                null,
                null,
                cancellationToken
            );

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
