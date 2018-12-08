using Newtonsoft.Json;

namespace Uni.Api.Web.Models.Responses
{
    [JsonObject]
    public class TeacherResponseModel : PersonResponseModel
    {
        public int FacultyId { get; set; }
    }
}
