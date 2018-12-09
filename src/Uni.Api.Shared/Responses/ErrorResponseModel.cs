using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class ErrorResponseModel
    {
        public string Status { get; set; }

        public string Message { get; set; }
    }
}
