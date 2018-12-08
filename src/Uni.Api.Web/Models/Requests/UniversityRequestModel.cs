using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests
{
    [PublicAPI]
    public class UniversityRequestModel
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}
