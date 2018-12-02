using System.Net;
using JetBrains.Annotations;

namespace Uni.Core.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {
        public NotFoundException([NotNull] string objectName, int id) : base(
            HttpStatusCode.NotFound,
            $"The {objectName} with id={id} wasn't found"
        )
        {
            Id = id;
        }

        public int Id { get; }
    }
}
