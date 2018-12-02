using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Uni.Infrastructure.Interfaces.Services
{
    public interface IBlobStorageUploader
    {
        [NotNull]
        Task<Uri> UploadImageToStorageAsync([NotNull] IFormFile file, CancellationToken cancellationToken = default);
    }
}
