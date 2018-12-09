using System;
using System.Collections.Generic;
using System.Reflection;
using Uni.Core.Utilities;

namespace Uni.Api.Web
{
    public static class Authorization
    {
        [RequiredScopes("api_main_scope")]
        [RequiredPermissions("account.read")]
        public const string ReadAccount = "ReadAccountPolicy";

        public static readonly AuthorizationPolicyInfo[] Info = GetInfo();

        private static AuthorizationPolicyInfo[] GetInfo()
        {
            var constantFields = ReflectionUtilities.GetConstantFields<string>(typeof(Authorization));

            var result = new List<AuthorizationPolicyInfo>(constantFields.Length);
            foreach (var constantField in constantFields)
            {
                var constantValue = constantField.GetRawConstantValue();
                if (!(constantValue is string value))
                {
                    continue;
                }

                var scopes = GetScopes(constantField);
                var permissions = GetPermissions(constantField);

                var policyInfo = new AuthorizationPolicyInfo(
                    value,
                    scopes,
                    permissions
                );

                result.Add(policyInfo);
            }

            return result.ToArray();
        }

        private static string[] GetScopes(MemberInfo constant)
        {
            var scopesAttribute = constant.GetCustomAttribute<RequiredScopesAttribute>();
            if (scopesAttribute == null)
            {
                return new string[]{};
            }

            return scopesAttribute.RequiredScopes;
        }

        private static string[] GetPermissions(MemberInfo constant)
        {
            var permissionsAttribute = constant.GetCustomAttribute<RequiredPermissionsAttribute>();
            if (permissionsAttribute == null)
            {
                return new string[] { };
            }

            return permissionsAttribute.RequiredPermissions;
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class RequiredScopesAttribute : Attribute
        {
            public RequiredScopesAttribute(params string[] requiredScopes)
            {
                RequiredScopes = requiredScopes;
            }

            public string[] RequiredScopes { get; }
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class RequiredPermissionsAttribute : Attribute
        {
            public RequiredPermissionsAttribute(params string[] requiredPermissions)
            {
                RequiredPermissions = requiredPermissions;
            }

            public string[] RequiredPermissions { get; }
        }
    }
}
