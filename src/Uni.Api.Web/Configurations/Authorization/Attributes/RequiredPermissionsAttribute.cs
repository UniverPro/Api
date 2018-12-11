using System;

namespace Uni.Api.Web.Configurations.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredPermissionsAttribute : Attribute
    {
        public RequiredPermissionsAttribute(params string[] requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }

        public string[] RequiredPermissions { get; }
    }
}
