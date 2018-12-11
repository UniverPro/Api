using System;
using System.Net;

namespace Uni.Api.Core.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCodeException(HttpStatusCode statusCode, string status) : this(
            statusCode,
            status,
            null,
            null
        )
        {
        }

        public HttpStatusCodeException(int statusCode, string status) : this(
            statusCode,
            status,
            null,
            null
        )
        {
        }

        public HttpStatusCodeException(
            HttpStatusCode statusCode,
            string status,
            string message
            ) : this(
            statusCode,
            status,
            message,
            null
        )
        {
        }

        public HttpStatusCodeException(
            int statusCode,
            string status,
            string message
            ) : this(
            statusCode,
            status,
            message,
            null
        )
        {
        }

        public HttpStatusCodeException(
            HttpStatusCode statusCode,
            string status,
            string message,
            Exception innerException
            ) : this(
            (int) statusCode,
            status,
            message,
            innerException
        )
        {
        }

        public HttpStatusCodeException(
            int statusCode,
            string status,
            string message,
            Exception innerException
            ) : base(message, innerException)
        {
            StatusCode = statusCode;
            Status = status;
        }

        public int StatusCode { get; }

        public string Status { get; }
    }
}
