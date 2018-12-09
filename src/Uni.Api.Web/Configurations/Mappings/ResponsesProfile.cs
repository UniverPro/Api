using AutoMapper;
using JetBrains.Annotations;
using Uni.Api.Shared.Responses;
using Uni.DataAccess.Models;

namespace Uni.Api.Web.Configurations.Mappings
{
    [UsedImplicitly]
    public sealed class ResponsesProfile : Profile
    {
        public ResponsesProfile()
        {
            CreateMap<Person, PersonResponseModel>()
                .IncludeAllDerived();

            CreateMap<Student, StudentResponseModel>();
            CreateMap<Teacher, TeacherResponseModel>();
        }
    }
}
