using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests
{
    [PublicAPI]
    public class SubjectRequestModel
    {
        public int GroupId { get; set; }

        public string Name { get; set; }
    }
}
