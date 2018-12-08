using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests
{
    [PublicAPI]
    public class TeacherRequestModel : PersonRequestModel
    {
        public int FacultyId { get; set; }
    }
}
