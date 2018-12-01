namespace Uni.WebApi.Models.Requests
{
    public class ListFacultiesRequestModel
    {
        /// <summary>
        ///     Filters results by university if value set
        /// </summary>
        public int? UniversityId { get; set; }

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