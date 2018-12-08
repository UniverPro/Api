using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public abstract class PersonResponseModel
    {
        public int Id { get; set; }

        public string AvatarPath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public int? UserId { get; set; }
    }
}
