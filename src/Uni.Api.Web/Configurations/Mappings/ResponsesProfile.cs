using System.Linq;
using AutoMapper;
using JetBrains.Annotations;
using Uni.Api.DataAccess.Models;
using Uni.Api.Shared.Responses;

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

            CreateMap<User, UserDetailsResponseModel>()
                .ForMember(
                    u => u.Roles,
                    opt => opt.MapFrom(u => u.UserRoles.Select(y => y.Role).ToList())
                );

            CreateMap<Role, RoleResponseModel>()
                .ForMember(
                    r => r.Permissions,
                    opt => opt.MapFrom(d => d.RolePermissions.Select(y => y.Permission).ToList())
                );
        }
    }
}
