using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests
{
    [PublicAPI]
    public class StudentRequestModel : PersonRequestModel
    {
        public int GroupId { get; set; }
    }
}
