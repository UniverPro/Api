using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Uni.Api.Core.Extensions;
using Uni.Api.Infrastructure.CQRS.Queries.Users.FindUserById;
using Uni.Api.Shared.Responses;
using Uni.Api.Web.Configurations.Authorization;

namespace Uni.Api.Web.Controllers
{
    [ApiVersion("1")]
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UserController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Returns the currently logged in user.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>User object</returns>
        [HttpGet]
        [Authorize(Policies.ReadAccount)]
        [SwaggerResponse(200, "The current user.", typeof(UserDetailsResponseModel))]
        public async Task<UserDetailsResponseModel> Get(
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var userId = User.SubjectId();

            // TODO: add custom query that should not return null - Single()
            var query = new FindUserByIdQuery(userId);

            var user = await _mediator.Send(query, cancellationToken);
            
            var response = _mapper.Map<UserDetailsResponseModel>(user);

            return response;
        }
    }
}