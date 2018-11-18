using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public class StudentResponseModel : PersonResponseModel
    {
        public int GroupId { get; set; }
    }
}
