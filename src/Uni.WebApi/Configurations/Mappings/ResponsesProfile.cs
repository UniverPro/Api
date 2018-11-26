using AutoMapper;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.WebApi.Models.Responses;

namespace Uni.WebApi.Configurations.Mappings
{
    [UsedImplicitly]
    internal sealed class ResponsesProfile : Profile
    {
        public ResponsesProfile()
        {
            CreateMap<Faculty, FacultyResponseModel>();
            CreateMap<Group, GroupResponseModel>();
            CreateMap<Person, PersonResponseModel>();
            CreateMap<Schedule, ScheduleResponseModel>();
            CreateMap<Student, StudentResponseModel>();
            CreateMap<Subject, SubjectResponseModel>();
            CreateMap<Teacher, TeacherResponseModel>();
            CreateMap<University, UniversityResponseModel>();
        }
    }
}