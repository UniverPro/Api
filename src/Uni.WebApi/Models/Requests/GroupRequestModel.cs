using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests
{
    [PublicAPI]
    public class GroupRequestModel
    {
        public int FacultyId { get; set; }

        public string Name { get; set; }

        public int CourseNumber { get; set; }
    }
}
