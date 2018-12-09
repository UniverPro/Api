using System;
using System.Net;
using JetBrains.Annotations;

namespace Uni.Core.Exceptions
{
    public class UnsupportedMediaTypeException : HttpStatusCodeException
    {
        public UnsupportedMediaTypeException([NotNull] string message) : this(
            message,
            null
        )
        {
        }

        public UnsupportedMediaTypeException([NotNull] string message, Exception innerException) : base(
            HttpStatusCode.UnsupportedMediaType,
            "Unsupported media type.",
            message,
            innerException
        )
        {
        }
    }
}
