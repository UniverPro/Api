using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.Infrastructure.CQRS.Commands.Schedules.CreateSchedule;
using Uni.Infrastructure.CQRS.Commands.Schedules.RemoveSchedule;
using Uni.Infrastructure.CQRS.Commands.Schedules.UpdateSchedule;
using Uni.Infrastructure.CQRS.Queries.Schedules.FindScheduleById;
using Uni.Infrastructure.CQRS.Queries.Schedules.FindSchedules;
using Uni.Infrastructure.Exceptions;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("schedules")]
    public class SchedulesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SchedulesController(
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        ///     Get all schedules
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get(
            [FromQuery] ListSchedulesRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindSchedulesQuery>(model);

            var schedules = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<ScheduleResponseModel>>(schedules);

            return response;
        }

        /// <summary>
        ///     Get all schedules with details
        /// </summary>
        /// <param name="model">Query specific filters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of schedule objects with details.</returns>
        [HttpGet("details")]
        public async Task<IEnumerable<ScheduleDetailsResponseModel>> GetDetails(
            [FromQuery] ListSchedulesRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _mapper.Map<FindSchedulesQuery>(model);

            var schedules = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<IEnumerable<ScheduleDetailsResponseModel>>(schedules);

            return response;
        }

        /// <summary>
        ///     Searches the schedule by id
        /// </summary>
        /// <param name="scheduleId">Schedule unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Schedule object</returns>
        [HttpGet("{scheduleId:int:min(1)}")]
        public async Task<ScheduleResponseModel> Get(int scheduleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = new FindScheduleByIdQuery(scheduleId);

            var schedule = await _mediator.Send(query, cancellationToken);

            if (schedule == null)
            {
                throw new NotFoundException(nameof(schedule), scheduleId);
            }

            var response = _mapper.Map<ScheduleResponseModel>(schedule);

            return response;
        }

        /// <summary>
        ///     Creates a new schedule
        /// </summary>
        /// <param name="model">Schedule object containing the data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created schedule object</returns>
        [HttpPost]
        public async Task<ScheduleResponseModel> Post(
            [FromForm] ScheduleRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new CreateScheduleCommand(
                model.SubjectId,
                model.TeacherId,
                model.StartTime,
                model.Duration,
                model.LessonType,
                model.AudienceNumber
            );

            var scheduleId = await _mediator.Send(command, cancellationToken);

            var query = new FindScheduleByIdQuery(scheduleId);

            var schedule = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<ScheduleResponseModel>(schedule);

            return response;
        }

        /// <summary>
        ///     Updates the schedule by id
        /// </summary>
        /// <param name="scheduleId">Schedule unique identifier</param>
        /// <param name="model">Schedule object containing the new data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated schedule object</returns>
        [HttpPut("{scheduleId:int:min(1)}")]
        public async Task<ScheduleResponseModel> Put(
            int scheduleId,
            [FromForm] ScheduleRequestModel model,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new UpdateScheduleCommand(
                scheduleId,
                model.SubjectId,
                model.TeacherId,
                model.StartTime,
                model.Duration,
                model.LessonType,
                model.AudienceNumber
            );

            await _mediator.Send(command, cancellationToken);

            var query = new FindScheduleByIdQuery(scheduleId);

            var schedule = await _mediator.Send(query, cancellationToken);

            var response = _mapper.Map<ScheduleResponseModel>(schedule);

            return response;
        }

        /// <summary>
        ///     Deletes the schedule by id
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="scheduleId">Schedule unique identifier</param>
        [HttpDelete("{scheduleId:int:min(1)}")]
        public async Task Delete(int scheduleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var command = new RemoveScheduleCommand(scheduleId);

            await _mediator.Send(command, cancellationToken);
        }
    }
}
