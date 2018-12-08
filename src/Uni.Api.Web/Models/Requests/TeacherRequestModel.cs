using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests
{
    [PublicAPI]
    public class TeacherRequestModel : PersonRequestModel
    {
        public int FacultyId { get; set; }
    }
}
