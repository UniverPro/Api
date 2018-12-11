using System.Net;
using JetBrains.Annotations;

namespace Uni.Api.Core.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {
        public NotFoundException([NotNull] string objectName, int? id = null) : base(
            HttpStatusCode.NotFound,
            "The object not found.",
            $"The {objectName} {(id.HasValue ? $"with id={id}":"")} wasn't found."
        )
        {
            Id = id;
        }

        public int? Id { get; }
    }
}
