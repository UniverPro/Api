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
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public StudentsController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all students
        /// </summary>
        /// <returns>List of student objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<StudentResponseModel>> Get()
        {
            var students = await _uniDbContext.Students.AsNoTracking()
                .Select(x => _mapper.Map<Student, StudentResponseModel>(x))
                .ToListAsync();

            return students;
        }

        /// <summary>
        ///     Searches the student by id
        /// </summary>
        /// <param name="id">Student unique identifier</param>
        /// <returns>Student object</returns>
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

        /// <summary>
        ///     Creates a new student
        /// </summary>
        /// <param name="model">Student object containing the data</param>
        /// <returns>Created student object</returns>
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

        /// <summary>
        ///     Updates the student by id
        /// </summary>
        /// <param name="id">Student unique identifier</param>
        /// <param name="model">Student object containing the new data</param>
        /// <returns>Updated student object</returns>
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

        /// <summary>
        ///     Deletes the student by id
        /// </summary>
        /// <param name="id">Student unique identifier</param>
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
