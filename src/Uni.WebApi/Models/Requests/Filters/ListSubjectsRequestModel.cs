using JetBrains.Annotations;

namespace Uni.WebApi.Models.Requests.Filters
{
    [PublicAPI]
    public class ListSubjectsRequestModel
    {
        /// <summary>
        ///     Filters results by group if value set
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        ///     Filters results by name if value set
        /// </summary>
        public string Name { get; set; }
    }
}
