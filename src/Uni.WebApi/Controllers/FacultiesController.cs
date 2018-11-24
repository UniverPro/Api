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
    [Route("api/v{version:apiVersion}/faculties")]
    public class FacultiesController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public FacultiesController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<FacultyResponseModel>> Get()
        {
            var faculties = await _uniDbContext.Faculties.AsNoTracking().Select(x => new FacultyResponseModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                ShortName = x.ShortName,
                UniversityId = x.UniversityId
            }).ToListAsync();

            return faculties;
        }

        [HttpGet("{id}")]
        public async Task<FacultyResponseModel> Get(int id)
        {
            var faculty = await _uniDbContext.Faculties.AsNoTracking().Select(x =>
                new FacultyResponseModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    ShortName = x.ShortName,
                    UniversityId = x.UniversityId
                }).SingleOrDefaultAsync(x => x.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException();
            }

            return faculty;
        }

        [HttpPost]
        public async Task<FacultyResponseModel> Post([FromBody] FacultyRequestModel model)
        {
            var faculty = new Faculty
            {
                UniversityId = model.UniversityId,
                Description = model.Description,
                Name = model.Name,
                ShortName = model.ShortName
            };

            var entityEntry = _uniDbContext.Faculties.Add(faculty);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new FacultyResponseModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                ShortName = entity.ShortName
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<FacultyResponseModel> Put(int id, [FromBody] FacultyRequestModel model)
        {
            var faculty = await _uniDbContext.Faculties.SingleOrDefaultAsync(x => x.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException();
            }

            faculty.Name = model.Name;
            faculty.ShortName = model.ShortName;
            faculty.Description = model.Description;

            await _uniDbContext.SaveChangesAsync();
            var response = new FacultyResponseModel
            {
                Id = faculty.Id,
                Description = faculty.Description,
                Name = faculty.Name,
                ShortName = faculty.ShortName,
                UniversityId = faculty.UniversityId
            };

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var faculty = await _uniDbContext.Faculties.SingleOrDefaultAsync(x => x.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Faculties.Remove(faculty);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
