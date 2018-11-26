using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public UniversitiesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<UniversityResponseModel>> Get()
        {
            var universities = await _uniDbContext.Universities.AsNoTracking()
                .Select(x => _mapper.Map<University, UniversityResponseModel>(x))
                .ToListAsync();

            return universities;
        }

        [HttpGet("{id}")]
        public async Task<UniversityResponseModel> Get(int id)
        {
            var university = await _uniDbContext.Universities.AsNoTracking()
                .Select(x => _mapper.Map<University, UniversityResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (university == null)
            {
                throw new NotFoundException();
            }

            return university;
        }

        [HttpPost]
        public async Task<UniversityResponseModel> Post([FromBody] UniversityRequestModel model)
        {
            var university = _mapper.Map<UniversityRequestModel, University>(model);

            var entityEntry = _uniDbContext.Universities.Add(university);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<University, UniversityResponseModel>(entity);

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

            _mapper.Map(model, university);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<University, UniversityResponseModel>(university);

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
