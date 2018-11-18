using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class UniversityRequestModel
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}
