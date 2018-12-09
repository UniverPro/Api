using System;
using System.Collections.Generic;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Uni.Api.Web.Configurations.Authorization.Attributes;
using Uni.Core.Utilities;

namespace Uni.Api.Web.Configurations.Authorization
{
    public static class AuthorizationConfiguration
    {
        public static void AddPolicies(
            [NotNull] this AuthorizationOptions options,
            [NotNull] [ItemNotNull] IEnumerable<AuthorizationPolicyInfo> policyInfos
            )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (policyInfos == null)
            {
                throw new ArgumentNullException(nameof(policyInfos));
            }

            AddPoliciesInternal(options, policyInfos);
        }

        public static void AddPolicies(
            [NotNull] this AuthorizationOptions options,
            [NotNull] Type type
            )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var policyInfos = GetPolicies(type);

            AddPoliciesInternal(options, policyInfos);
        }
        
        private static IEnumerable<AuthorizationPolicyInfo> GetPolicies(Type type)
        {
            var constantFields = ReflectionUtilities.GetConstantFields<string>(type);

            var result = new List<AuthorizationPolicyInfo>(constantFields.Length);
            foreach (var constantField in constantFields)
            {
                var constantValue = constantField.GetRawConstantValue();
                if (!(constantValue is string value))
                {
                    continue;
                }

                var policyInfo = PolicyInfoFromMemberInfoAndName(value, constantField);

                result.Add(policyInfo);
            }

            return result;
        }

        private static AuthorizationPolicyInfo PolicyInfoFromMemberInfoAndName(string name, MemberInfo constantField)
        {
            var scopesAttribute = constantField.GetCustomAttribute<RequiredScopesAttribute>();
            var scopes = scopesAttribute?.RequiredScopes;

            var permissionsAttribute = constantField.GetCustomAttribute<RequiredPermissionsAttribute>();
            var permissions = permissionsAttribute?.RequiredPermissions;

            var policyInfo = new AuthorizationPolicyInfo(
                name,
                scopes,
                permissions
            );
            return policyInfo;
        }

        private static void AddPoliciesInternal(
            AuthorizationOptions options,
            IEnumerable<AuthorizationPolicyInfo> policyInfos
            )
        {
            var builder = new AuthorizationPolicyBuilder(
                IdentityServerAuthenticationDefaults.AuthenticationScheme
            );

            foreach (var policyInfo in policyInfos)
            {
                var policy = builder
                    .RequireAuthenticatedUser()
                    .RequireScope(policyInfo.Scopes)
                    .RequireClaim("permissions", policyInfo.Permissions)
                    .Build();

                options.AddPolicy(policyInfo.Name, policy);
            }
        }
    }
}
