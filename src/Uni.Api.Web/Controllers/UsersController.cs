using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Uni.Core.Exceptions;
using Uni.Infrastructure.CQRS.Commands.Users.CreateUser;
using Uni.Infrastructure.CQRS.Commands.Users.RemoveUser;
using Uni.Infrastructure.CQRS.Commands.Users.UpdateUser;
using Uni.Infrastructure.CQRS.Queries.Users.FindUserById;
using Uni.Infrastructure.CQRS.Queries.Users.FindUsers;
using Uni.Api.Web.Models.Requests;
using Uni.Api.Web.Models.Requests.Filters;
using Uni.Api.Web.Models.Responses;

namespace Uni.Api.Web.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all users
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of user objects.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserResponseModel>))]
        public async Task<IEnumerable<UserResponseModel>> GetUsers(
            [FromQuery] ListUsersRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindUsersQuery>(model);

            var users = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<UserResponseModel>>(users);

            return response;
        }

        /// <summary>
        ///     Searches the user by id
        /// </summary>
        /// <param name="userId">User unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>User object</returns>
        [HttpGet("{userId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(UserResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<UserResponseModel> GetUser(
            int userId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindUserByIdQuery(userId);

            var user = await _mediator.Send(query, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(user), userId);
            }

            var response = _mapper.Map<UserResponseModel>(user);

            return response;
        }

        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <param name="model">User object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created user object</returns>
        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(UserResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<UserResponseModel> PostUser(
            [FromForm] UserRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = _mapper.Map<CreateUserCommand>(model);

            var userId = await _mediator.Send(command, cancellationToken);

            var query = new FindUserByIdQuery(userId);
            var user = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<UserResponseModel>(user);

            return response;
        }

        /// <summary>
        ///     Updates the user by id
        /// </summary>
        /// <param name="userId">User unique identifier</param>
        /// <param name="model">User object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated user object</returns>
        [HttpPut("{userId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(UserResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<UserResponseModel> PutUser(
            int userId,
            [FromForm] UserRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateUserCommand(
                userId,
                model.Login,
                model.Password,
                model.PersonId
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindUserByIdQuery(userId);

            var user = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<UserResponseModel>(user);

            return response;
        }

        /// <summary>
        ///     Deletes the user by id
        /// </summary>
        /// <param name="userId">User unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("{userId:int:min(1)}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Not Found")]
        public async Task DeleteUser(
            int userId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveUserCommand(userId);

            await _mediator.Send(command, cancellationToken);
        }
    }
}
