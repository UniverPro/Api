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
    [Route("api/v{version:apiVersion}/universities")]
    public class UniversitiesController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;

        public UniversitiesController([NotNull] UniDbContext uniDbContext)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        [HttpGet]
        public async Task<IEnumerable<UniversityResponseModel>> Get()
        {
            var universities = await _uniDbContext.Universities.AsNoTracking().Select(x => new UniversityResponseModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                ShortName = x.ShortName
            }).ToListAsync();

            return universities;
        }

        [HttpGet("{id}")]
        public async Task<UniversityResponseModel> Get(int id)
        {
            var university = await _uniDbContext.Universities.AsNoTracking().Select(x => new UniversityResponseModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                ShortName = x.ShortName
            }).SingleOrDefaultAsync(x => x.Id == id);

            if (university == null)
            {
                throw new NotFoundException();
            }

            return university;
        }

        [HttpPost]
        public async Task<UniversityResponseModel> Post([FromBody] UniversityRequestModel model)
        {
            var university = new University
            {
                Name = model.Name,
                ShortName = model.ShortName,
                Description = model.Description
            };

            var entityEntry = _uniDbContext.Universities.Add(university);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = new UniversityResponseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ShortName = entity.ShortName,
                Description = entity.Description
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<UniversityResponseModel> Put(int id, [FromBody] UniversityRequestModel model)
        {
            var university = await _uniDbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);

            if (university == null)
            {
                throw new NotFoundException();
            }

            university.Name = model.Name;
            university.ShortName = model.ShortName;
            university.Description = model.Description;

            await _uniDbContext.SaveChangesAsync();
            var response = new UniversityResponseModel
            {
                Id = university.Id,
                Name = university.Name,
                ShortName = university.ShortName,
                Description = university.Description
            };

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var university = await _uniDbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);

            if (university == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Universities.Remove(university);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
