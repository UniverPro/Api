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
using Uni.Infrastructure.CQRS.Commands.Teachers.CreateTeacher;
using Uni.Infrastructure.CQRS.Commands.Teachers.RemoveTeacher;
using Uni.Infrastructure.CQRS.Commands.Teachers.UpdateTeacher;
using Uni.Infrastructure.CQRS.Queries.Teachers.FindTeacherById;
using Uni.Infrastructure.CQRS.Queries.Teachers.FindTeachers;
using Uni.Api.Web.Models.Requests;
using Uni.Api.Web.Models.Requests.Filters;
using Uni.Api.Web.Models.Responses;

namespace Uni.Api.Web.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TeachersController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all teachers
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of teacher objects.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<TeacherResponseModel>))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<IEnumerable<TeacherResponseModel>> GetTeachers(
            [FromQuery] ListTeachersRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindTeachersQuery>(model);

            var teachers = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<TeacherResponseModel>>(teachers);

            return response;
        }

        /// <summary>
        ///     Searches the teacher by id
        /// </summary>
        /// <param name="teacherId">Teacher unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Teacher object</returns>
        [HttpGet("{teacherId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(TeacherResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<TeacherResponseModel> GetTeacher(int teacherId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindTeacherByIdQuery(teacherId);

            var teacher = await _mediator.Send(query, cancellationToken);

            if (teacher == null)
            {
                throw new NotFoundException(nameof(teacher), teacherId);
            }

            var response = _mapper.Map<TeacherResponseModel>(teacher);

            return response;
        }

        /// <summary>
        ///     Creates a new teacher
        /// </summary>
        /// <param name="model">Teacher object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created teacher object</returns>
        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(TeacherResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<TeacherResponseModel> PostTeacher(
            [FromForm] TeacherRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateTeacherCommand(
                model.FirstName,
                model.LastName,
                model.MiddleName,
                model.Email,
                model.Avatar,
                model.FacultyId
            );

            var teacherId = await _mediator.Send(command, cancellationToken);

            var query = new FindTeacherByIdQuery(teacherId);

            var teacher = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<TeacherResponseModel>(teacher);

            return response;
        }

        /// <summary>
        ///     Updates the teacher by id
        /// </summary>
        /// <param name="teacherId">Teacher unique identifier</param>
        /// <param name="model">Teacher object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated teacher object</returns>
        [HttpPut("{teacherId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(TeacherResponseModel))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<TeacherResponseModel> PutTeacher(
            int teacherId,
            [FromForm] TeacherRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateTeacherCommand(
                teacherId,
                model.FirstName,
                model.LastName,
                model.MiddleName,
                model.Email,
                model.Avatar,
                model.FacultyId
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindTeacherByIdQuery(teacherId);

            var teacher = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<TeacherResponseModel>(teacher);

            return response;
        }

        /// <summary>
        ///     Deletes the teacher by id
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="teacherId">Teacher unique identifier</param>
        [HttpDelete("{teacherId:int:min(1)}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Not Found")]
        public async Task DeleteTeacher(int teacherId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveTeacherCommand(teacherId);

            await _mediator.Send(command, cancellationToken);
        }
    }
}
