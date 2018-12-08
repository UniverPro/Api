using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Refit;
using Uni.Api.Shared.Requests;
using Uni.Api.Shared.Requests.Filters;
using Uni.Api.Shared.Responses;

namespace Uni.Api.Client
{
    [PublicAPI]
    public interface IUniApiClient
    {
        [ItemNotNull]
        [Post("/users")]
        Task<UserResponseModel> CreateUserAsync(
            [NotNull] [Body(BodySerializationMethod.UrlEncoded)]
            UserRequestModel request,
            CancellationToken token = default
            );

        [ItemNotNull]
        [Put("/users/{userId}")]
        Task<UserResponseModel> UpdateUserAsync(
            [AliasAs("userId")] int userId,
            [NotNull] [Body(BodySerializationMethod.UrlEncoded)]
            UserRequestModel request,
            CancellationToken token = default
            );

        [Delete("/users/{userId}")]
        Task DeleteUserAsync([AliasAs("userId")] int userId, CancellationToken token = default);

        [ItemNotNull]
        [Get("/users/{userId}")]
        Task<UserResponseModel> FindUserByIdAsync([AliasAs("userId")] int userId, CancellationToken token = default);

        [ItemNotNull]
        [Get("/users")]
        Task<UserResponseModel> FindUserByLoginAndPasswordAsync(
            [NotNull] [AliasAs("login")] string login,
            [NotNull] [AliasAs("password")] string password,
            CancellationToken token = default
            );
    }
}
