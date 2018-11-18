using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public class TeacherResponseModel : PersonResponseModel
    {
        public int FacultyId { get; set; }
    }
}
