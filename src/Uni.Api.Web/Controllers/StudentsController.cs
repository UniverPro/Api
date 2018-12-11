using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Uni.Api.Core.Exceptions;
using Uni.Api.Infrastructure.CQRS.Commands.Students.CreateStudent;
using Uni.Api.Infrastructure.CQRS.Commands.Students.RemoveStudent;
using Uni.Api.Infrastructure.CQRS.Commands.Students.UpdateStudent;
using Uni.Api.Infrastructure.CQRS.Queries.Students.FindStudentById;
using Uni.Api.Infrastructure.CQRS.Queries.Students.FindStudents;
using Uni.Api.Shared.Requests;
using Uni.Api.Shared.Requests.Filters;
using Uni.Api.Shared.Responses;

namespace Uni.Api.Web.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public StudentsController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all students
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of student objects.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<StudentResponseModel>))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<IEnumerable<StudentResponseModel>> GetStudents(
            [FromQuery] ListStudentsRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindStudentsQuery>(model);

            var students = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<StudentResponseModel>>(students);

            return response;
        }

        /// <summary>
        ///     Searches the student by id
        /// </summary>
        /// <param name="studentId">Student unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Student object</returns>
        [HttpGet("{studentId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(StudentResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<StudentResponseModel> GetStudent(int studentId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindStudentByIdQuery(studentId);

            var student = await _mediator.Send(query, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException(nameof(student), studentId);
            }

            var response = _mapper.Map<StudentResponseModel>(student);

            return response;
        }

        /// <summary>
        ///     Creates a new student
        /// </summary>
        /// <param name="model">Student object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created student object</returns>
        [HttpPost]
        [SwaggerResponse(200, "Success", typeof(StudentResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<StudentResponseModel> PostStudent(
            [FromForm] StudentRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateStudentCommand(
                model.FirstName,
                model.LastName,
                model.MiddleName,
                model.Email,
                model.Avatar,
                model.GroupId
            );

            var studentId = await _mediator.Send(command, cancellationToken);

            var query = new FindStudentByIdQuery(studentId);

            var student = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<StudentResponseModel>(student);

            return response;
        }

        /// <summary>
        ///     Updates the student by id
        /// </summary>
        /// <param name="studentId">Student unique identifier</param>
        /// <param name="model">Student object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated student object</returns>
        [HttpPut("{studentId:int:min(1)}")]
        [SwaggerResponse(200, "Success", typeof(StudentResponseModel))]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task<StudentResponseModel> PutStudent(
            int studentId,
            [FromForm] StudentRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateStudentCommand(
                studentId,
                model.FirstName,
                model.LastName,
                model.MiddleName,
                model.Email,
                model.Avatar,
                model.GroupId
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindStudentByIdQuery(studentId);

            var student = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<StudentResponseModel>(student);

            return response;
        }

        /// <summary>
        ///     Deletes the student by id
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="studentId">Student unique identifier</param>
        [HttpDelete("{studentId:int:min(1)}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Not Found", typeof(ErrorResponseModel))]
        public async Task DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveStudentCommand(studentId);

            await _mediator.Send(command, cancellationToken);
        }
    }
}
