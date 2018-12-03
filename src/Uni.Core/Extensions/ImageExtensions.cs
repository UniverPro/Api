using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Uni.Core.Exceptions;

namespace Uni.Core.Extensions
{
    public static class ImageExtensions
    {
        private static readonly ImageCodecInfo[] Decoders = ImageCodecInfo.GetImageDecoders();

        public static string AssertFileIsWebFriendlyImageAndGetExtension(Stream stream)
        {
            using (var image = Image.FromStream(stream))
            {
                var validWebImageFormats = new[] {ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif};
                if (!TryGetCodecInfo(image, out var codecInfo) || !validWebImageFormats.Contains(image.RawFormat))
                {
                    throw new UnsupportedMediaTypeException("The file has unsupported media type");
                }

                return codecInfo.FilenameExtension;
            }
        }

        private static bool TryGetCodecInfo(Image img, out ImageCodecInfo codecInfo)
        {
            codecInfo = Decoders.FirstOrDefault(decoder => img.RawFormat.Guid == decoder.FormatID);

            return codecInfo != null;
        }
    }
}
