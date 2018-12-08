using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class GroupRequestModel
    {
        public int FacultyId { get; set; }

        public string Name { get; set; }

        public int CourseNumber { get; set; }
    }
}
