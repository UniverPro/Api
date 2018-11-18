using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public class SubjectResponseModel
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public string Name { get; set; }
    }
}
