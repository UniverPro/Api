using JetBrains.Annotations;

namespace Uni.Api.Web.Models.Requests.Filters
{
    // TODO: add validator
    [PublicAPI]
    public class FindUserByLoginAndPasswordRequestModel
    {
        public string Login { get; set; }
        
        public string Password { get; set; }
    }
}