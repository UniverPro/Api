using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests.Filters
{
    [PublicAPI]
    public class ListUsersRequestModel
    {
        /// <summary>
        ///     Filters results by logins if value set
        /// </summary>
        public string Login { get; set; }
    }
}