using System;

namespace Uni.Api.Web.Configurations.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredScopesAttribute : Attribute
    {
        public RequiredScopesAttribute(params string[] requiredScopes)
        {
            RequiredScopes = requiredScopes;
        }

        public string[] RequiredScopes { get; }
    }
}