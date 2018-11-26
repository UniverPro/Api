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
    [Route("api/v{version:apiVersion}/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IMapper _mapper;

        public GroupsController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<GroupResponseModel>> Get()
        {
            var groups = await _uniDbContext.Groups.AsNoTracking()
                .Select(x => _mapper.Map<Group, GroupResponseModel>(x))
                .ToListAsync();

            return groups;
        }

        [HttpGet("{id}")]
        public async Task<GroupResponseModel> Get(int id)
        {
            var group = await _uniDbContext.Groups.AsNoTracking()
                .Select(x => _mapper.Map<Group, GroupResponseModel>(x))
                .SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            return group;
        }

        [HttpPost]
        public async Task<GroupResponseModel> Post([FromForm] GroupRequestModel model)
        {
            var group = _mapper.Map<GroupRequestModel, Group>(model);

            var entityEntry = _uniDbContext.Groups.Add(group);

            await _uniDbContext.SaveChangesAsync();

            var entity = entityEntry.Entity;

            var response = _mapper.Map<Group, GroupResponseModel>(entity);
            
            return response;
        }

        [HttpPut("{id}")]
        public async Task<GroupResponseModel> Put(int id, [FromForm] GroupRequestModel model)
        {
            var group = await _uniDbContext.Groups.SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(model, group);

            await _uniDbContext.SaveChangesAsync();

            var response = _mapper.Map<Group, GroupResponseModel>(group);

            return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var group = await _uniDbContext.Groups.SingleOrDefaultAsync(x => x.Id == id);

            if (group == null)
            {
                throw new NotFoundException();
            }

            _uniDbContext.Groups.Remove(group);

            await _uniDbContext.SaveChangesAsync();
        }
    }
}
