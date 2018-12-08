using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Uni.WebApi.Models.Requests
{
    [PublicAPI]
    public abstract class PersonRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
