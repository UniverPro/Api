using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uni.DataAccess.Models;
using Uni.Infrastructure.CQRS.Commands.Common.Create;
using Uni.Infrastructure.CQRS.Commands.Common.Delete;
using Uni.Infrastructure.CQRS.Queries.Common.FindAll;
using Uni.Infrastructure.CQRS.Queries.Common.FindById;
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
        private readonly IMediator _mediator;

        public FacultiesController([NotNull] IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IEnumerable<FacultyResponseModel>> Get(CancellationToken cancellationToken)
        {
            var entities = await _mediator.Send(new FindAllQuery<Faculty>(), cancellationToken);

            var response = entities.Select(x => new FacultyResponseModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                ShortName = x.ShortName,
                UniversityId = x.UniversityId
            }).ToList();

            return response;
        }

        [HttpGet("{id}")]
        public async Task<FacultyResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var query = new FindByIdQuery<Faculty>(id);
            var entity = await _mediator.Send(query, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            var response = new FacultyResponseModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                ShortName = entity.ShortName,
                UniversityId = entity.UniversityId
            };

            return response;
        }

        [HttpPost]
        public async Task<FacultyResponseModel> Post([FromBody] FacultyRequestModel model, CancellationToken cancellationToken)
        {
            var faculty = new Faculty
            {
                UniversityId = model.UniversityId,
                Description = model.Description,
                Name = model.Name,
                ShortName = model.ShortName
            };

            var command = new CreateCommand<Faculty>(faculty);
            var entity = await _mediator.Send(command, cancellationToken);

            var response = new FacultyResponseModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Name = entity.Name,
                ShortName = entity.ShortName
            };

            return response;
        }

        [HttpPut("{id}")]
        public async Task<FacultyResponseModel> Put(int id, [FromBody] FacultyRequestModel model, CancellationToken cancellationToken)
        {
            var query = new FindByIdQuery<Faculty>(id);
            var entity = await _mediator.Send(query, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            var faculty = new Faculty
            {
                UniversityId = model.UniversityId,
                Description = model.Description,
                Name = model.Name,
                ShortName = model.ShortName
            };

            var command = new UpdateCommand<Faculty>(id, faculty);
            var entity = await _mediator.Send(query, cancellationToken);

            //var entity = await _uniDbContext.Faculties.SingleOrDefaultAsync(x => x.Id == id);

            //if (entity == null)
            //{
            //    throw new NotFoundException();
            //}

            //entity.Name = model.Name;
            //entity.ShortName = model.ShortName;
            //entity.Description = model.Description;

            //await _uniDbContext.SaveChangesAsync();
            //var response = new FacultyResponseModel
            //{
            //    Id = entity.Id,
            //    Description = entity.Description,
            //    Name = entity.Name,
            //    ShortName = entity.ShortName,
            //    UniversityId = entity.UniversityId
            //};

            //return response;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var query = new FindByIdQuery<Faculty>(id);
            var entity = await _mediator.Send(query, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException();
            }

            var command = new DeleteCommand<Faculty>(entity);
            await _mediator.Send(command, cancellationToken);
        }
    }
}
