using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class TeacherResponseModel : PersonResponseModel
    {
        public int FacultyId { get; set; }
    }
}
