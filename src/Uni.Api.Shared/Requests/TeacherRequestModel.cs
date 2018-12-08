using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class TeacherRequestModel : PersonRequestModel
    {
        public int FacultyId { get; set; }
    }
}
