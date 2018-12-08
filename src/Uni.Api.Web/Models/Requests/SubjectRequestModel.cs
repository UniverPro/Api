using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests
{
    [PublicAPI]
    public class SubjectRequestModel
    {
        public int GroupId { get; set; }

        public string Name { get; set; }
    }
}
