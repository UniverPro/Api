using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class TeacherRequestModel : PersonRequestModel
    {
        public int FacultyId { get; set; }
    }
}
