﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.Infrastructure.CQRS.Commands.Subjects.CreateSubject;
using Uni.Infrastructure.CQRS.Commands.Subjects.RemoveSubject;
using Uni.Infrastructure.CQRS.Commands.Subjects.UpdateSubject;
using Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjectById;
using Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjects;
using Uni.Infrastructure.Exceptions;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SubjectsController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all subjects
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of subject objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<SubjectResponseModel>> Get(
            [FromQuery] ListSubjectsRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindSubjectsQuery>(model);

            var subjects = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<SubjectResponseModel>>(subjects);

            return response;
        }

        /// <summary>
        ///     Searches the subject by id
        /// </summary>
        /// <param name="subjectId">Subject unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Subject object</returns>
        [HttpGet("{subjectId:int:min(1)}")]
        public async Task<SubjectResponseModel> Get(int subjectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindSubjectByIdQuery(subjectId);
            var subject = await _mediator.Send(query, cancellationToken);

            if (subject == null)
            {
                throw new NotFoundException(nameof(subject), subjectId);
            }

            var response = _mapper.Map<SubjectResponseModel>(subject);

            return response;
        }

        /// <summary>
        ///     Creates a new subject
        /// </summary>
        /// <param name="model">Subject object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created subject object</returns>
        [HttpPost]
        public async Task<SubjectResponseModel> Post(
            [FromForm] SubjectRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateSubjectCommand(
                model.GroupId,
                model.Name
            );

            var subjectId = await _mediator.Send(command, cancellationToken);

            var query = new FindSubjectByIdQuery(subjectId);
            var subject = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<SubjectResponseModel>(subject);

            return response;
        }

        /// <summary>
        ///     Updates the subject by id
        /// </summary>
        /// <param name="subjectId">Subject unique identifier</param>
        /// <param name="model">Subject object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated subject object</returns>
        [HttpPut("{subjectId:int:min(1)}")]
        public async Task<SubjectResponseModel> Put(
            int subjectId,
            [FromForm] SubjectRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateSubjectCommand(
                subjectId,
                model.GroupId,
                model.Name
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindSubjectByIdQuery(subjectId);
            var subject = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<SubjectResponseModel>(subject);

            return response;
        }

        /// <summary>
        ///     Deletes the subject by id
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="subjectId">Subject unique identifier</param>
        [HttpDelete("{subjectId:int:min(1)}")]
        public async Task Delete(int subjectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveSubjectCommand(subjectId);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
