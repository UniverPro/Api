using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class StudentResponseModel : PersonResponseModel
    {
        public int GroupId { get; set; }
    }
}
