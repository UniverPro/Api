using System.Net;
using JetBrains.Annotations;

namespace Uni.Infrastructure.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {
        public int Id { get; }

        public NotFoundException([NotNull] string objectName, int id) : base(HttpStatusCode.NotFound, $"The {objectName} with id={id} wasn't found")
        {
            Id = id;
        }
    }
}
