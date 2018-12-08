using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests
{
    [PublicAPI]
    public class StudentRequestModel : PersonRequestModel
    {
        public int GroupId { get; set; }
    }
}
