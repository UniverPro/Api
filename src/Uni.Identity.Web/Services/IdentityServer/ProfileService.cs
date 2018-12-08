using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using JetBrains.Annotations;
using Uni.DataAccess.Models;

namespace Uni.Identity.Web.Services.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService([NotNull] IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = SubjectIdToInt(context.Subject.GetSubjectId());
            var user = await _userService.FindUserByIdAsync(userId);
            var claims = user.ToClaims();
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = SubjectIdToInt(context.Subject.GetSubjectId());
            var user = await _userService.FindUserByIdAsync(userId);
            context.IsActive = user != null /* && user.IsActive*/;
        }

        /// <summary>
        ///     Конвертирует уникальный идентификатор субъекта из строкового представления - в число.
        /// </summary>
        /// <param name="subjectId">Уникальный идентификатор субъекта.</param>
        /// <returns></returns>
        private static int SubjectIdToInt([NotNull] string subjectId)
        {
            if (subjectId == null)
            {
                throw new ArgumentNullException(nameof(subjectId));
            }

            var id = int.Parse(
                subjectId,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture
            );

            return id;
        }
    }

    public static class PersonExtensions
    {
        public static IEnumerable<Claim> ToClaims([NotNull] this Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.GivenName, person.FirstName),
                new Claim(JwtClaimTypes.FamilyName, person.LastName)
            };

            if (!string.IsNullOrWhiteSpace(person.AvatarPath))
            {
                claims.Add(new Claim(JwtClaimTypes.Picture, person.AvatarPath));
            }

            return claims;
        }
    }
}
