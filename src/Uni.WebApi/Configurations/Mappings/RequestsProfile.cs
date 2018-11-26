using AutoMapper;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Configurations.Mappings
{
    [UsedImplicitly]
    internal sealed class RequestsProfile : Profile
    {
        public RequestsProfile()
        {
            CreateMap<FacultyRequestModel, Faculty>();
            CreateMap<GroupRequestModel, Group>();
            CreateMap<PersonRequestModel, Person>();
            CreateMap<ScheduleRequestModel, Schedule>();
            CreateMap<StudentRequestModel, Student>();
            CreateMap<SubjectRequestModel, Subject>();
            CreateMap<TeacherRequestModel, Teacher>();
            CreateMap<UniversityRequestModel, University>();
        }
    }
}
