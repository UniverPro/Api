using Newtonsoft.Json;
using Uni.Api.Shared.Converters;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class UserDetailsResponseModel
    {
        public int Id { get; set; }

        public string Login { get; set; }

        [JsonConverter(typeof(PersonConverter))]
        public PersonResponseModel Person { get; set; }
    }
}
