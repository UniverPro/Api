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
    [Route("api/v{version:apiVersion}/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UniDbContext _uniDbContext;

        public GroupsController([NotNull] UniDbContext uniDbContext, [NotNull] IMapper mapper)
        {
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Get all groups
        /// </summary>
        /// <returns>List of group objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<GroupResponseModel>> Get()
        {
            var groups = await _uniDbContext.Groups.AsNoTracking()
                .Select(x => _mapper.Map<Group, GroupResponseModel>(x))
                .ToListAsync();

            return groups;
        }

        /// <summary>
        ///     Searches the group by id
        /// </summary>
        /// <param name="id">Group unique identifier</param>
        /// <returns>Group object</returns>
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

        /// <summary>
        ///     Creates a new group
        /// </summary>
        /// <param name="model">Group object containing the data</param>
        /// <returns>Created group object</returns>
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

        /// <summary>
        ///     Updates the group by id
        /// </summary>
        /// <param name="id">Group unique identifier</param>
        /// <param name="model">Group object containing the new data</param>
        /// <returns>Updated group object</returns>
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

        /// <summary>
        ///     Deletes the group by id
        /// </summary>
        /// <param name="id">Group unique identifier</param>
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
