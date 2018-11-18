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
    [Route("api/v{version:apiVersion}/teachers")]
    public class TeachersController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<TeacherResponseModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<TeacherResponseModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<TeacherResponseModel> Post([FromBody] TeacherRequestModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<TeacherResponseModel> Put(int id, [FromBody] TeacherRequestModel model)
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
