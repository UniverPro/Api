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
    [Route("api/v{version:apiVersion}/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public SubjectsController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<SubjectResponseModel>> Get()
        {
            var subjects = await _uniDbContext.Subjects.AsNoTracking().Select(x => new SubjectResponseModel
            {
                Id = x.Id,
                GroupId = x.GroupId,
                Name = x.Name,
                TeacherId = x.TeacherId
            }).ToListAsync();

            return subjects;
        }

        [HttpGet("{id}")]
        public async Task<SubjectResponseModel> Get(int id)
        {
            var subject = await _uniDbContext.Subjects.AsNoTracking().Select(x => new SubjectResponseModel
            {
                Id = x.Id,
                GroupId = x.GroupId,
                Name = x.Name,
                TeacherId = x.TeacherId
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new NotFoundException();
            }

            return subject;
        }

        [HttpPost]
        public async Task<SubjectResponseModel> Post([FromBody] SubjectRequestModel model)
        {
            var subject = new Subject
            {
                Name = model.Name,
                GroupId = model.GroupId,
                TeacherId = model.TeacherId
            };

            var entityEntry = _uniDbContext.Subjects.Add(subject);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new SubjectResponseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                GroupId = entity.GroupId,
                TeacherId = entity.TeacherId
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<SubjectResponseModel> Put(int id, [FromBody] SubjectRequestModel model)
        {
            var subject = await _uniDbContext.Subjects.SingleOrDefaultAsync(x => x.Id == id);

            if (subject == null)
            {
                throw new NotFoundException();
            }

            subject.Name = model.Name;
            subject.GroupId = model.GroupId;
            subject.TeacherId = model.TeacherId;

            await _uniDbContext.SaveChangesAsync();
            var response = new SubjectResponseModel
            {
                Id = subject.Id,
                Name = subject.Name,
                GroupId = subject.GroupId,
                TeacherId = subject.TeacherId
            };

            return response;
        }

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
