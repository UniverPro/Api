using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Uni.Api.Core.Exceptions;

namespace Uni.Api.Core.Extensions
{
    public static class ImageExtensions
    {
        private static readonly ImageCodecInfo[] Decoders = ImageCodecInfo.GetImageDecoders();

        [NotNull]
        public static string AssertFileIsWebFriendlyImageAndGetFormatDescription([NotNull] Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var image = Image.FromStream(stream))
            {
                var validWebImageFormats = new[] {ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif};
                if (!TryGetCodecInfo(image, out var codecInfo) || !validWebImageFormats.Contains(image.RawFormat))
                {
                    throw new UnsupportedMediaTypeException("The file has unsupported media type");
                }

                return codecInfo.FormatDescription.ToLower();
            }
        }

        private static bool TryGetCodecInfo(Image img, out ImageCodecInfo codecInfo)
        {
            codecInfo = Decoders.FirstOrDefault(decoder => img.RawFormat.Guid == decoder.FormatID);

            return codecInfo != null;
        }
    }
}
