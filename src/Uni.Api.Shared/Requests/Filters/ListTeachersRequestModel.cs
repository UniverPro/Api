using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests.Filters
{
    [PublicAPI]
    public class ListTeachersRequestModel
    {
        /// <summary>
        ///     Filters results by faculty if value set
        /// </summary>
        public int? FacultyId { get; set; }

        /// <summary>
        ///     Filters results by first name if value set
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Filters results by last name if value set
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Filters results by middle name if value set
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        ///     Filters results by email if value set
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Filters results by avatar path if value set
        /// </summary>
        public string AvatarPath { get; set; }
    }
}
