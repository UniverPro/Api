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
    [Route("api/v{version:apiVersion}/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public TeachersController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<TeacherResponseModel>> Get()
        {
            var teachers = await _uniDbContext.Teachers.AsNoTracking().Select(x => new TeacherResponseModel
            {
                Id = x.Id,
                AvatarPath = x.AvatarPath,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                FacultyId = x.FacultyId
            }).ToListAsync();

            return teachers;
        }

        [HttpGet("{id}")]
        public async Task<TeacherResponseModel> Get(int id)
        {
            var teacher = await _uniDbContext.Teachers.AsNoTracking().Select(x => new TeacherResponseModel
            {
                Id = x.Id,
                AvatarPath = x.AvatarPath,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                FacultyId = x.FacultyId
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                throw new NotFoundException();
            }

            return teacher;
        }

        [HttpPost]
        public async Task<TeacherResponseModel> Post([FromBody] TeacherRequestModel model)
        {
            var teacher = new Teacher
            {
                AvatarPath = model.AvatarPath,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                FacultyId = model.FacultyId
            };

            var entityEntry = _uniDbContext.Teachers.Add(teacher);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new TeacherResponseModel
            {
                Id = entity.Id,
                AvatarPath = entity.AvatarPath,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                FacultyId = entity.FacultyId
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<TeacherResponseModel> Put(int id, [FromBody] TeacherRequestModel model)
        {
            var teacher = await _uniDbContext.Teachers.SingleOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                throw new NotFoundException();
            }

            teacher.AvatarPath = model.AvatarPath;
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.MiddleName = model.MiddleName;
            teacher.FacultyId = model.FacultyId;

            await _uniDbContext.SaveChangesAsync();
            var response = new TeacherResponseModel
            {
                Id = teacher.Id,
                AvatarPath = teacher.AvatarPath,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                FacultyId = teacher.FacultyId
            };

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var teacher = await _uniDbContext.Teachers.SingleOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Teachers.Remove(teacher);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
