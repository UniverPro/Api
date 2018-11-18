using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uni.WebApi.Models.Requests;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/subjects")]
    public class SubjectsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<SubjectResponseModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<SubjectResponseModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<SubjectResponseModel> Post([FromBody] SubjectRequestModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<SubjectResponseModel> Put(int id, [FromBody] SubjectRequestModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
