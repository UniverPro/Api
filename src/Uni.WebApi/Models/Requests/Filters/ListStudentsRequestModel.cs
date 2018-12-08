using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests.Filters
{
    [PublicAPI]
    public class ListStudentsRequestModel
    {
        /// <summary>
        ///     Filters results by group if value set
        /// </summary>
        public int? GroupId { get; set; }

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
