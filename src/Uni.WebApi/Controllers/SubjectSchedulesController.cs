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
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/subjects/{subjectId}/schedules")]
    public class SubjectSchedulesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public SubjectSchedulesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all schedules for specified subject
        /// </summary>
        /// <param name="subjectId">Subject unique identifier</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get(int subjectId)
        {
            var subjectExists = await _uniDbContext.Subjects.AnyAsync(x => x.Id == subjectId);

            if (!subjectExists)
            {
                throw new NotFoundException();
            }

            var schedules = await _uniDbContext.Schedules.AsNoTracking()
                .Where(x => x.SubjectId == subjectId)
                .Select(x => _mapper.Map<Schedule, ScheduleResponseModel>(x))
                .ToListAsync();

            return schedules;
        }
    }
}
