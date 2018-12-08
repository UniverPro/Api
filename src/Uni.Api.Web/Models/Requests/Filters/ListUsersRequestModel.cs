using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests.Filters
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