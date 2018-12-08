using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class StudentRequestModel : PersonRequestModel
    {
        public int GroupId { get; set; }
    }
}
