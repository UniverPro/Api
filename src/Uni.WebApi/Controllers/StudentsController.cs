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
    [Route("api/v{version:apiVersion}/students")]
    public class StudentsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public StudentsController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<StudentResponseModel>> Get()
        {
            var students = await _uniDbContext.Students.AsNoTracking()
                .Select(x => _mapper.Map<Student, StudentResponseModel>(x))
                .ToListAsync();

            return students;
        }

        [HttpGet("{id}")]
        public async Task<StudentResponseModel> Get(int id)
        {
            var student = await _uniDbContext.Students.AsNoTracking()
                .Select(x => _mapper.Map<Student, StudentResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new NotFoundException();
            }

            return student;
        }

        [HttpPost]
        public async Task<StudentResponseModel> Post([FromForm] StudentRequestModel model)
        {
            var student = _mapper.Map<StudentRequestModel, Student>(model);

            var entityEntry = _uniDbContext.Students.Add(student);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<Student, StudentResponseModel>(entity);

            return response;
        }

        [HttpPut("{id}")]
        public async Task<StudentResponseModel> Put(int id, [FromForm] StudentRequestModel model)
        {
            var student = await _uniDbContext.Students.SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(model, student);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Student, StudentResponseModel>(student);

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
