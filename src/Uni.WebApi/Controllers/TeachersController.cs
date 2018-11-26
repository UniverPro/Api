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
    [Route("api/v{version:apiVersion}/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public TeachersController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<TeacherResponseModel>> Get()
        {
            var teachers = await _uniDbContext.Teachers.AsNoTracking()
                .Select(x => _mapper.Map<Teacher, TeacherResponseModel>(x))
                .ToListAsync();

            return teachers;
        }

        [HttpGet("{id}")]
        public async Task<TeacherResponseModel> Get(int id)
        {
            var teacher = await _uniDbContext.Teachers.AsNoTracking()
                .Select(x => _mapper.Map<Teacher, TeacherResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                throw new NotFoundException();
            }

            return teacher;
        }

        [HttpPost]
        public async Task<TeacherResponseModel> Post([FromForm] TeacherRequestModel model)
        {
            var teacher = _mapper.Map<TeacherRequestModel, Teacher>(model);

            var entityEntry = _uniDbContext.Teachers.Add(teacher);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<Teacher, TeacherResponseModel>(entity);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<TeacherResponseModel> Put(int id, [FromForm] TeacherRequestModel model)
        {
            var teacher = await _uniDbContext.Teachers.SingleOrDefaultAsync(x => x.Id == id);

            if (teacher == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(model, teacher);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Teacher, TeacherResponseModel>(teacher);

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
