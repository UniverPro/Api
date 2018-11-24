using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Data;
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

        public SchedulesController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get()
        {
            var schedules = await _uniDbContext.Schedules.AsNoTracking().Select(x => new ScheduleResponseModel
            {
                Id = x.Id,
                SubjectId = x.SubjectId,
                Duration = x.Duration,
                StartTime = x.StartTime
            }).ToListAsync();

            return schedules;
        }

        [HttpGet("{id}")]
        public async Task<ScheduleResponseModel> Get(int id)
        {
            var schedule = await _uniDbContext.Schedules.AsNoTracking().Select(x => new ScheduleResponseModel
            {
                Id = x.Id,
                SubjectId = x.SubjectId,
                Duration = x.Duration,
                StartTime = x.StartTime
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            return schedule;
        }

        [HttpPost]
        public async Task<ScheduleResponseModel> Post([FromBody] ScheduleRequestModel model)
        {
            var schedule = new Schedule
            {
                SubjectId = model.SubjectId,
                StartTime = model.StartTime,
                Duration = model.Duration
            };

            var entityEntry = _uniDbContext.Schedules.Add(schedule);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new ScheduleResponseModel
            {
                Id = entity.Id,
                SubjectId = entity.SubjectId,
                StartTime = entity.StartTime,
                Duration = entity.Duration
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<ScheduleResponseModel> Put(int id, [FromBody] ScheduleRequestModel model)
        {
            var schedule = await _uniDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            schedule.SubjectId = model.SubjectId;
            schedule.StartTime = model.StartTime;
            schedule.Duration = model.Duration;

            await _uniDbContext.SaveChangesAsync();
            var response = new ScheduleResponseModel
            {
                Id = schedule.Id,
                SubjectId = schedule.SubjectId,
                StartTime = schedule.StartTime,
                Duration = schedule.Duration
            };

            return response;
        }

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
