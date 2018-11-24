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
    [Route("api/v{version:apiVersion}/students")]
    public class StudentsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public StudentsController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<StudentResponseModel>> Get()
        {
            var students = await _uniDbContext.Students.AsNoTracking().Select(x => new StudentResponseModel
            {
                Id = x.Id,
                AvatarPath = x.AvatarPath,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                GroupId = x.GroupId
            }).ToListAsync();

            return students;
        }

        [HttpGet("{id}")]
        public async Task<StudentResponseModel> Get(int id)
        {
            var student = await _uniDbContext.Students.AsNoTracking().Select(x => new StudentResponseModel
            {
                Id = x.Id,
                AvatarPath = x.AvatarPath,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                GroupId = x.GroupId
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new NotFoundException();
            }

            return student;
        }

        [HttpPost]
        public async Task<StudentResponseModel> Post([FromBody] StudentRequestModel model)
        {
            var student = new Student
            {
                AvatarPath = model.AvatarPath,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                GroupId = model.GroupId
            };

            var entityEntry = _uniDbContext.Students.Add(student);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new StudentResponseModel
            {
                Id = entity.Id,
                AvatarPath = entity.AvatarPath,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                GroupId = entity.GroupId
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<StudentResponseModel> Put(int id, [FromBody] StudentRequestModel model)
        {
            var student = await _uniDbContext.Students.SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new NotFoundException();
            }

            student.AvatarPath = model.AvatarPath;
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.MiddleName = model.MiddleName;
            student.GroupId = model.GroupId;

            await _uniDbContext.SaveChangesAsync();
            var response = new StudentResponseModel
            {
                Id = student.Id,
                AvatarPath = student.AvatarPath,
                FirstName = student.FirstName,
                LastName = student.LastName,
                MiddleName = student.MiddleName,
                GroupId = student.GroupId
            };

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var student = await _uniDbContext.Students.SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Students.Remove(student);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
