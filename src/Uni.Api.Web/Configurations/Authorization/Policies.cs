using Uni.Api.Web.Configurations.Authorization.Attributes;

namespace Uni.Api.Web.Configurations.Authorization
{
    public static class Policies
    {
        [RequiredScopes(Scopes.Main)]
        [RequiredPermissions(Permissions.ReadAccount)]
        public const string ReadAccount = "ReadAccountPolicy";
    }
}
