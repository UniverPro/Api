using Newtonsoft.Json;

namespace Uni.Api.Web.Models.Responses
{
    [JsonObject]
    public class StudentResponseModel : PersonResponseModel
    {
        public int GroupId { get; set; }
    }
}
