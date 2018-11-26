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

        /// <summary>
        ///     Get all universities
        /// </summary>
        /// <returns>List of university objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<UniversityResponseModel>> Get()
        {
            var universities = await _uniDbContext.Universities.AsNoTracking()
                .Select(x => _mapper.Map<University, UniversityResponseModel>(x))
                .ToListAsync();

            return universities;
        }

        /// <summary>
        ///     Searches the university by id
        /// </summary>
        /// <param name="id">University unique identifier</param>
        /// <returns>University object</returns>
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

        /// <summary>
        ///     Get all faculties from specified university
        /// </summary>
        /// <param name="universityId">University unique identifier</param>
        /// <returns>List of faculty objects.</returns>
        [HttpGet("{universityId}/faculties")]
        public async Task<IEnumerable<FacultyResponseModel>> GetFaculties(int universityId)
        {
            var universityExists = await _uniDbContext.Universities.AnyAsync(x => x.Id == universityId);

            if (!universityExists)
            {
                throw new NotFoundException();
            }

            var faculties = await _uniDbContext.Faculties.AsNoTracking()
                .Where(x => x.UniversityId == universityId)
                .Select(x => _mapper.Map<Faculty, FacultyResponseModel>(x))
                .ToListAsync();

            return faculties;
        }

        /// <summary>
        ///     Creates a new university
        /// </summary>
        /// <param name="model">University object containing the data</param>
        /// <returns>Created university object</returns> 
        [HttpPost]
        public async Task<UniversityResponseModel> Post([FromForm] UniversityRequestModel model)
        {
            var university = _mapper.Map<UniversityRequestModel, University>(model);

            var entityEntry = _uniDbContext.Universities.Add(university);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<University, UniversityResponseModel>(entity);

            return response;
        }

        /// <summary>
        ///     Updates the university by id
        /// </summary>
        /// <param name="id">University unique identifier</param>
        /// <param name="model">University object containing the new data</param>
        /// <returns>Updated university object</returns>
        [HttpPut("{id}")]
        public async Task<UniversityResponseModel> Put(int id, [FromForm] UniversityRequestModel model)
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

        /// <summary>
        ///     Deletes the university by id
        /// </summary>
        /// <param name="id">University unique identifier</param>
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
