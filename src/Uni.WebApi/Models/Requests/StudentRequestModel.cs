using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class StudentRequestModel : PersonRequestModel
    {
        public int GroupId { get; set; }
    }
}
