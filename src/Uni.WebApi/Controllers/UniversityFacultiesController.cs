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
    [Route("api/v{version:apiVersion}/universities/{universityId}/faculties")]
    public class UniversityFacultiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public UniversityFacultiesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all faculties from specified university
        /// </summary>
        /// <param name="universityId">University unique identifier</param>
        /// <returns>List of faculty objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<FacultyResponseModel>> Get(int universityId)
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
    }
}
