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
    [Route("api/v{version:apiVersion}/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public GroupsController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<GroupResponseModel>> Get()
        {
            var groups = await _uniDbContext.Groups.AsNoTracking().Select(x => new GroupResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                CourseNumber = x.CourseNumber,
                FacultyId = x.FacultyId
            }).ToListAsync();

            return groups;
        }

        [HttpGet("{id}")]
        public async Task<GroupResponseModel> Get(int id)
        {
            var group = await _uniDbContext.Groups.AsNoTracking().Select(x => new GroupResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                CourseNumber = x.CourseNumber,
                FacultyId = x.FacultyId
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            return group;
        }

        [HttpPost]
        public async Task<GroupResponseModel> Post([FromBody] GroupRequestModel model)
        {
            var group = new Group
            {
                Name = model.Name,
                CourseNumber = model.CourseNumber,
                FacultyId = model.FacultyId
            };

            var entityEntry = _uniDbContext.Groups.Add(group);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new GroupResponseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CourseNumber = entity.CourseNumber,
                FacultyId = entity.FacultyId
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<GroupResponseModel> Put(int id, [FromBody] GroupRequestModel model)
        {
            var group = await _uniDbContext.Groups.SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            group.Name = model.Name;
            group.CourseNumber = model.CourseNumber;
            group.FacultyId = model.FacultyId;

            await _uniDbContext.SaveChangesAsync();
            var response = new GroupResponseModel
            {
                Id = group.Id,
                Name = group.Name,
                CourseNumber = group.CourseNumber,
                FacultyId = group.FacultyId
            };

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var group = await _uniDbContext.Groups.SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Groups.Remove(group);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
