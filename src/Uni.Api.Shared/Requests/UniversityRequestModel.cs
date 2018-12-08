using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class UniversityRequestModel
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }
    }
}
