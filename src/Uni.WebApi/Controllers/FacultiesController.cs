using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.Infrastructure.CQRS.Commands.Faculties.CreateFaculty;
using Uni.Infrastructure.CQRS.Commands.Faculties.RemoveFaculty;
using Uni.Infrastructure.CQRS.Commands.Faculties.UpdateFaculty;
using Uni.Infrastructure.CQRS.Queries.Faculties.FindFaculties;
using Uni.Infrastructure.CQRS.Queries.Faculties.FindFacultyById;
using Uni.Infrastructure.CQRS.Queries.Universities.CheckUniversityExists;
using Uni.Infrastructure.Exceptions;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("faculties")]
    public class FacultiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public FacultiesController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all faculties
        /// </summary>
        /// <param name="universityId">Filter results by university</param>
        /// <param name="name">Filter results by Name</param>
        /// <param name="shortName">Filter results by ShortName</param>
        /// <param name="description">Filter results by Description</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of faculty objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FacultyResponseModel>), 200)]
        [ProducesResponseType(404)]
        public async Task<IEnumerable<FacultyResponseModel>> Get(
            int? universityId,
            string name,
            string shortName,
            string description,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (universityId != null)
            {
                var universityQuery = new CheckUniversityExistsQuery(universityId.Value);

                var universityExists = await _mediator.Send(universityQuery, cancellationToken);

                if (!universityExists)
                {
                    throw new NotFoundException();
                }
            }

            var query = new FindFacultiesQuery(
                universityId,
                name,
                shortName,
                description
            );

            var faculties = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<FacultyResponseModel>>(faculties);

            return response;
        }

        /// <summary>
        ///     Searches the faculty by id
        /// </summary>
        /// <param name="facultyId">Faculty unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Faculty object</returns>
        [HttpGet("{facultyId:int:min(1)}")]
        [ProducesResponseType(typeof(FacultyResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<FacultyResponseModel> Get(
            int facultyId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindFacultyByIdQuery(facultyId);
            var faculty = await _mediator.Send(query, cancellationToken);

            if (faculty == null)
            {
                throw new NotFoundException();
            }
            
            var response = _mapper.Map<FacultyResponseModel>(faculty);

            return response;
        }

        /// <summary>
        ///     Creates a new faculty
        /// </summary>
        /// <param name="model">Faculty object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created faculty object</returns>
        [HttpPost]
        [ProducesResponseType(typeof(FacultyResponseModel), 200)]
        public async Task<FacultyResponseModel> Post(
            [FromForm] FacultyRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateFacultyCommand(
                model.Name,
                model.ShortName,
                model.Description,
                model.UniversityId
            );

            var facultyId = await _mediator.Send(command, cancellationToken);

            var query = new FindFacultyByIdQuery(facultyId);
            var faculty = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<FacultyResponseModel>(faculty);

            return response;
        }

        /// <summary>
        ///     Updates the faculty by id
        /// </summary>
        /// <param name="facultyId">Faculty unique identifier</param>
        /// <param name="model">Faculty object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated faculty object</returns>
        [HttpPut("{facultyId:int:min(1)}")]
        [ProducesResponseType(typeof(FacultyResponseModel), 200)]
        [ProducesResponseType(404)]
        public async Task<FacultyResponseModel> Put(
            int facultyId,
            [FromForm] FacultyRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateFacultyCommand(
                facultyId,
                model.Name,
                model.ShortName,
                model.Description,
                model.UniversityId
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindFacultyByIdQuery(facultyId);
            var faculty = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<FacultyResponseModel>(faculty);

            return response;
        }

        /// <summary>
        ///     Deletes the faculty by id
        /// </summary>
        /// <param name="facultyId">Faculty unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("{facultyId:int:min(1)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task Delete(
            int facultyId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveFacultyCommand(facultyId);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
