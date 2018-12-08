using Newtonsoft.Json;

namespace Uni.Api.Web.Models.Responses
{
    [JsonObject]
    public class FacultyResponseModel
    {
        public int Id { get; set; }

        public int UniversityId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}
