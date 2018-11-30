using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.Infrastructure.CQRS.Commands.Students.CreateStudent;
using Uni.Infrastructure.CQRS.Commands.Students.RemoveStudent;
using Uni.Infrastructure.CQRS.Commands.Students.UpdateStudent;
using Uni.Infrastructure.CQRS.Queries.Students.FindStudentById;
using Uni.Infrastructure.CQRS.Queries.Students.FindStudents;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
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
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of student objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<StudentResponseModel>> Get(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindStudentsQuery();
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
        public async Task<StudentResponseModel> Get(int studentId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindStudentByIdQuery(studentId);
            var student = await _mediator.Send(query, cancellationToken);

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
        public async Task<StudentResponseModel> Post(
            [FromForm] StudentRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateStudentCommand(
                model.FirstName,
                model.LastName,
                model.MiddleName,
                model.AvatarPath,
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
        public async Task<StudentResponseModel> Put(
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
                model.AvatarPath,
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
        public async Task Delete(int studentId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveStudentCommand(studentId);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
