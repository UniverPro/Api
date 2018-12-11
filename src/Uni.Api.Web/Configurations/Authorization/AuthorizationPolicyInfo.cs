using System;
using JetBrains.Annotations;

namespace Uni.Api.Web.Configurations.Authorization
{
    public class AuthorizationPolicyInfo
    {
        public AuthorizationPolicyInfo(
            [NotNull] string name,
            [CanBeNull] string[] scopes,
            [CanBeNull] string[] permissions
            )
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Name = name;
            Scopes = scopes;
            Permissions = permissions;
        }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public string[] Scopes { get; }

        [CanBeNull]
        public string[] Permissions { get; }
    }
}
