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
    [Route("api/v{version:apiVersion}/schedules")]
    public class SchedulesController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ScheduleResponseModel>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ScheduleResponseModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ScheduleResponseModel> Post([FromBody] ScheduleRequestModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<ScheduleResponseModel> Put(int id, [FromBody] ScheduleRequestModel model)
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
