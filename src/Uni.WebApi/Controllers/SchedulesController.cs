using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Exceptions;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/schedules")]
    public class SchedulesController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public SchedulesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all schedules
        /// </summary>
        /// <returns>List of schedule objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get()
        {
            var schedules = await _uniDbContext.Schedules.AsNoTracking()
                .Select(x => _mapper.Map<Schedule, ScheduleResponseModel>(x))
                .ToListAsync();

            return schedules;
        }

        /// <summary>
        ///     Searches the schedule by id
        /// </summary>
        /// <param name="id">Schedule unique identifier</param>
        /// <returns>Schedule object</returns>
        [HttpGet("{id}")]
        public async Task<ScheduleResponseModel> Get(int id)
        {
            var schedule = await _uniDbContext.Schedules.AsNoTracking()
                .Select(x => _mapper.Map<Schedule, ScheduleResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            return schedule;
        }
        
        /// <summary>
        ///     Searches the schedule by day date
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="date">Date to pick the schedules</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet("{groupId}")]
        public async Task<IEnumerable<ScheduleResponseModel>> GetByDate(int groupId, [FromForm] DateTime date)
        {
            var dayTruncated = date.Date;
            var schedules = await _uniDbContext.Schedules.AsNoTracking()
                .Where(x => groupId == x.Subject.GroupId)
                .Where(x => dayTruncated == x.StartTime)
                .Select(x => _mapper.Map<Schedule, ScheduleResponseModel>(x))
                .ToListAsync();

            return schedules;
        }

        /// <summary>
        ///     Creates a new schedule
        /// </summary>
        /// <param name="model">Schedule object containing the data</param>
        /// <returns>Created schedule object</returns>
        [HttpPost]
        public async Task<ScheduleResponseModel> Post([FromForm] ScheduleRequestModel model)
        {
            var schedule = _mapper.Map<ScheduleRequestModel, Schedule>(model);

            var entityEntry = _uniDbContext.Schedules.Add(schedule);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<Schedule, ScheduleResponseModel>(entity);
            
            return response;
        }

        /// <summary>
        ///     Updates the schedule by id
        /// </summary>
        /// <param name="id">Schedule unique identifier</param>
        /// <param name="model">Schedule object containing the new data</param>
        /// <returns>Updated schedule object</returns>
        [HttpPut("{id}")]
        public async Task<ScheduleResponseModel> Put(int id, [FromForm] ScheduleRequestModel model)
        {
            var schedule = await _uniDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(model, schedule);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Schedule, ScheduleResponseModel>(schedule);

            return response;
        }

        /// <summary>
        ///     Deletes the schedule by id
        /// </summary>
        /// <param name="id">Schedule unique identifier</param>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var schedule = await _uniDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Schedules.Remove(schedule);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
