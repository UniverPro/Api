using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/groups")]
    public class GroupsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<GroupResponseModel>> Get(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<GroupResponseModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<GroupResponseModel> Post([FromBody] GroupRequestModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<GroupResponseModel> Put(int id, [FromBody] GroupRequestModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
