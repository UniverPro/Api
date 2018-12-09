namespace Uni.Api.Web
{
    public class AuthorizationPolicyInfo
    {
        public AuthorizationPolicyInfo(
            string name,
            string[] scopes,
            string[] permissions)
        {
            Name = name;
            Scopes = scopes;
            Permissions = permissions;
        }
 
        public string Name { get; }
        public string[] Scopes { get; }
        public string[] Permissions { get; }
    }
}