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
    [Route("api/v{version:apiVersion}/students")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<StudentResponseModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<StudentResponseModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<StudentResponseModel> Post([FromBody] StudentRequestModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<StudentResponseModel> Put(int id, [FromBody] StudentRequestModel model)
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
