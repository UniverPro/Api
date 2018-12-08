using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests.Filters
{
    [PublicAPI]
    public class ListGroupsRequestModel
    {
        /// <summary>
        ///     Filters results by faculty if value set
        /// </summary>
        public int? FacultyId { get; set; }

        /// <summary>
        ///     Filters results by name if value set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Filters results by course number if value set
        /// </summary>
        public int? CourseNumber { get; set; }
    }
}
