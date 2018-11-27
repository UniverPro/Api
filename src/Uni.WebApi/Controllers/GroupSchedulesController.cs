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
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/groups/{groupId}/schedules")]
    public class GroupSchedulesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public GroupSchedulesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get the schedule of selected group by date
        /// </summary>
        /// <param name="groupId">Group unique identifier</param>
        /// <param name="date">Date to pick the schedules</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get(int groupId, [FromForm] DateTime? date)
        {
            var query = _uniDbContext.Schedules.AsNoTracking()
                .Where(x => groupId == x.Subject.GroupId);

            if (date.HasValue)
            {
                var dayTruncated = date.Value.Date;
                query = query.Where(x => dayTruncated == x.StartTime.Date);
            }

            var schedules = await query
                .Select(x => _mapper.Map<Schedule, ScheduleResponseModel>(x))
                .ToListAsync();

            return schedules;
        }

        /// <summary>
        ///     Get the schedule of selected group by date with details
        /// </summary>
        /// <param name="groupId">Group unique identifier</param>
        /// <param name="date">Date to pick the schedules</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet("details")]
        public async Task<IEnumerable<ScheduleDetailsResponseModel>> GetDetails(int groupId, [FromForm] DateTime? date)
        {
            var query = _uniDbContext.Schedules
                .AsNoTracking()
                .Where(x => groupId == x.Subject.GroupId);

            if (date.HasValue)
            {
                var dayTruncated = date.Value.Date;
                query = query.Where(x => dayTruncated == x.StartTime.Date);
            }

            var schedules = await query
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Select(x => _mapper.Map<Schedule, ScheduleDetailsResponseModel>(x))
                .ToListAsync();

            return schedules;
        }
    }
}
