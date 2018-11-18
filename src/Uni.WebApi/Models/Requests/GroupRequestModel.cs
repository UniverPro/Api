using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class GroupRequestModel
    {
        public int FacultyId { get; set; }

        public string Name { get; set; }

        public int CourseNumber { get; set; }
    }
}
