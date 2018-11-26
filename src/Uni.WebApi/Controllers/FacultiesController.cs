﻿using System;
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
    [Route("api/v{version:apiVersion}/faculties")]
    public class FacultiesController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public FacultiesController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<FacultyResponseModel>> Get()
        {
            var faculties = await _uniDbContext.Faculties.AsNoTracking()
                .Select(x => _mapper.Map<Faculty, FacultyResponseModel>(x))
                .ToListAsync();

            return faculties;
        }

        [HttpGet("{id}")]
        public async Task<FacultyResponseModel> Get(int id)
        {
            var faculty = await _uniDbContext.Faculties.AsNoTracking()
                .Select(x => _mapper.Map<Faculty, FacultyResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (faculty == null)
            {
                throw new NotFoundException();
            }

            return faculty;
        }

        [HttpPost]
        public async Task<FacultyResponseModel> Post([FromBody] FacultyRequestModel model)
        {
            var faculty = _mapper.Map<FacultyRequestModel, Faculty>(model);

            var entityEntry = _uniDbContext.Faculties.Add(faculty);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<Faculty, FacultyResponseModel>(entity);

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

            _mapper.Map(model, faculty);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Faculty, FacultyResponseModel>(faculty);

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
