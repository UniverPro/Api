using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Teachers.FindTeacherById
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
