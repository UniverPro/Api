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
    [Route("api/v{version:apiVersion}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public SubjectsController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all subjects
        /// </summary>
        /// <returns>List of subject objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<SubjectResponseModel>> Get()
        {
            var subjects = await _uniDbContext.Subjects.AsNoTracking()
                .Select(x => _mapper.Map<Subject, SubjectResponseModel>(x))
                .ToListAsync();

            return subjects;
        }

        /// <summary>
        ///     Searches the subject by id
        /// </summary>
        /// <param name="id">Subject unique identifier</param>
        /// <returns>Subject object</returns>
        [HttpGet("{id}")]
        public async Task<SubjectResponseModel> Get(int id)
        {
            var subject = await _uniDbContext.Subjects.AsNoTracking()
                .Select(x => _mapper.Map<Subject, SubjectResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new NotFoundException();
            }

            return subject;
        }

        /// <summary>
        ///     Get all schedules for specified subject
        /// </summary>
        /// <param name="subjectId">Subject unique identifier</param>
        /// <returns>List of schedule objects.</returns>
        [HttpGet("{subjectId}/schedules")]
        public async Task<IEnumerable<ScheduleResponseModel>> GetSchedules(int subjectId)
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

        /// <summary>
        ///     Creates a new subject
        /// </summary>
        /// <param name="model">Schedule object containing the data</param>
        /// <returns>Created subject object</returns>
        [HttpPost]
        public async Task<SubjectResponseModel> Post([FromForm] SubjectRequestModel model)
        {
            var subject = _mapper.Map<SubjectRequestModel, Subject>(model);

            var entityEntry = _uniDbContext.Subjects.Add(subject);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;
            
            var response = _mapper.Map<Subject, SubjectResponseModel>(entity);

            return response;
        }

        /// <summary>
        ///     Updates the subject by id
        /// </summary>
        /// <param name="id">Subject unique identifier</param>
        /// <param name="model">Subject object containing the new data</param>
        /// <returns>Updated subject object</returns>
        [HttpPut("{id}")]
        public async Task<SubjectResponseModel> Put(int id, [FromForm] SubjectRequestModel model)
        {
            var subject = await _uniDbContext.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(model, subject);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Subject, SubjectResponseModel>(subject);

            return response;
        }

        /// <summary>
        ///     Deletes the subject by id
        /// </summary>
        /// <param name="id">Subject unique identifier</param>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var subject = await _uniDbContext.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Subjects.Remove(subject);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
