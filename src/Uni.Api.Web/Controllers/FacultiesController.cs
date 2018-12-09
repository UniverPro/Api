using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Uni.Api.Shared.Requests;
using Uni.Api.Shared.Requests.Filters;
using Uni.Api.Shared.Responses;
using Uni.Core.Exceptions;
using Uni.Infrastructure.CQRS.Commands.Faculties.CreateFaculty;
using Uni.Infrastructure.CQRS.Commands.Faculties.RemoveFaculty;
using Uni.Infrastructure.CQRS.Commands.Faculties.UpdateFaculty;
using Uni.Infrastructure.CQRS.Queries.Faculties.FindFaculties;
using Uni.Infrastructure.CQRS.Queries.Faculties.FindFacultyById;

namespace Uni.Api.Web.Controllers
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
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of faculty objects.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<FacultyResponseModel>))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<IEnumerable<FacultyResponseModel>> GetFaculties(
            [FromQuery] ListFacultiesRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindFacultiesQuery>(model);

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
        [SwaggerResponse(200, "Success", typeof(FacultyResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<FacultyResponseModel> GetFaculty(
            int facultyId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindFacultyByIdQuery(facultyId);

            var faculty = await _mediator.Send(query, cancellationToken);

            if (faculty == null)
            {
                throw new NotFoundException(nameof(faculty), facultyId);
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
        [SwaggerResponse(200, "Success", typeof(FacultyResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<FacultyResponseModel> PostFaculty(
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
        [SwaggerResponse(200, "Success", typeof(FacultyResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<FacultyResponseModel> PutFaculty(
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
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task DeleteFaculty(
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
