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
using Uni.Api.Client;
using Uni.Api.Shared.Responses;

namespace Uni.Identity.Web.Services.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUniApiClient _uniApiClient;

        public ProfileService([NotNull] IUniApiClient uniApiClient)
        {
            _uniApiClient = uniApiClient ?? throw new ArgumentNullException(nameof(uniApiClient));
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = SubjectIdToInt(context.Subject.GetSubjectId());
            var user = await _uniApiClient.FindUserByIdAsync(userId);
            var claims = user.ToClaims();
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = SubjectIdToInt(context.Subject.GetSubjectId());
            var dummy = await _uniApiClient.FindUserByIdAsync(userId);
            context.IsActive = true /* && user.IsActive*/;
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
        public static IEnumerable<Claim> ToClaims([NotNull] this UserResponseModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.PreferredUserName, user.Login)
            };

            //if (!string.IsNullOrWhiteSpace(user.AvatarPath))
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Picture, user.AvatarPath));
            //}

            return claims;
        }
    }
}
