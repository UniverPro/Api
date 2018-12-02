using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.Core.Exceptions;
using Uni.Infrastructure.CQRS.Commands.Universities.CreateUniversity;
using Uni.Infrastructure.CQRS.Commands.Universities.RemoveUniversity;
using Uni.Infrastructure.CQRS.Commands.Universities.UpdateUniversity;
using Uni.Infrastructure.CQRS.Queries.Universities.FindUniversities;
using Uni.Infrastructure.CQRS.Queries.Universities.FindUniversityById;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("universities")]
    public class UniversitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UniversitiesController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all universities
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of university objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UniversityResponseModel>), 200)]
        public async Task<IEnumerable<UniversityResponseModel>> GetList(
            [FromQuery] ListUniversitiesRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindUniversitiesQuery>(model);

            var universities = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<UniversityResponseModel>>(universities);

            return response;
        }

        /// <summary>
        ///     Searches the university by id
        /// </summary>
        /// <param name="universityId">University unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>University object</returns>
        [HttpGet("{universityId:int:min(1)}")]
        [ProducesResponseType(typeof(UniversityResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<UniversityResponseModel> Get(
            int universityId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindUniversityByIdQuery(universityId);

            var university = await _mediator.Send(query, cancellationToken);

            if (university == null)
            {
                throw new NotFoundException(nameof(university), universityId);
            }

            var response = _mapper.Map<UniversityResponseModel>(university);

            return response;
        }

        /// <summary>
        ///     Creates a new university
        /// </summary>
        /// <param name="model">University object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created university object</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UniversityResponseModel), 200)]
        public async Task<UniversityResponseModel> Post(
            [FromBody] UniversityRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateUniversityCommand(model.Name, model.ShortName, model.Description);

            var universityId = await _mediator.Send(command, cancellationToken);

            var query = new FindUniversityByIdQuery(universityId);

            var university = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<UniversityResponseModel>(university);

            return response;
        }

        /// <summary>
        ///     Updates the university by id
        /// </summary>
        /// <param name="universityId">University unique identifier</param>
        /// <param name="model">University object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated university object</returns>
        [HttpPut("{universityId:int:min(1)}")]
        [ProducesResponseType(typeof(UniversityResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<UniversityResponseModel> Put(
            int universityId,
            [FromBody] UniversityRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateUniversityCommand(
                universityId,
                model.Name,
                model.ShortName,
                model.Description
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindUniversityByIdQuery(universityId);

            var university = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<UniversityResponseModel>(university);

            return response;
        }

        /// <summary>
        ///     Deletes the university by id
        /// </summary>
        /// <param name="universityId">University unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("{universityId:int:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task Delete(
            int universityId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveUniversityCommand(universityId);

            await _mediator.Send(command, cancellationToken);
        }
    }
}
