using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class GroupResponseModel
    {
        public int Id { get; set; }

        public int FacultyId { get; set; }

        public string Name { get; set; }

        public int CourseNumber { get; set; }
    }
}
