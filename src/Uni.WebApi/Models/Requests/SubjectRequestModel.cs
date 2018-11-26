using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class SubjectRequestModel
    {
        public int GroupId { get; set; }

        public int ScheduleId { get; set; }

        public string Name { get; set; }
    }
}
