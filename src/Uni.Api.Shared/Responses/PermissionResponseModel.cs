using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class PermissionResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
