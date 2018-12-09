using System;
using System.Globalization;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Uni.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int SubjectId([NotNull] this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var subject = user.FindFirst("sub")?.Value ?? throw new InvalidOperationException("sub claim is missing");
            var userId = int.Parse(subject, CultureInfo.InvariantCulture);

            return userId;
        }
    }
}
