using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class SubjectResponseModel
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }
    }
}
