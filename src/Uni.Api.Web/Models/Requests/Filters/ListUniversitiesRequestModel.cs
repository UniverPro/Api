using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests.Filters
{
    [PublicAPI]
    public class ListUniversitiesRequestModel
    {
        /// <summary>
        ///     Filters results by name if value set
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Filters results by short name if value set
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        ///     Filters results by description if value set
        /// </summary>
        public string Description { get; set; }
    }
}
