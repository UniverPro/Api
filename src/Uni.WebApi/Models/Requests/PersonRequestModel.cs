using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public abstract class PersonRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
