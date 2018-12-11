using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Teachers.FindTeacherById
{
    public class FindTeacherByIdQuery : IQuery<Teacher>
    {
        public FindTeacherByIdQuery(int teacherId)
        {
            TeacherId = teacherId;
        }

        public int TeacherId { get; }
    }
}
