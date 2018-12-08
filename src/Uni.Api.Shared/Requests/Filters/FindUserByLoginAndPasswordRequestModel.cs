using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests.Filters
{
    [PublicAPI]
    public class FindUserByLoginAndPasswordRequestModel
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
