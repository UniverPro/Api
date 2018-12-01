using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Teachers.CheckTeacherExists
{
    public class CheckTeacherExistsQuery : IQuery<bool>
    {
        public CheckTeacherExistsQuery(int teacherId)
        {
            TeacherId = teacherId;
        }

        public int TeacherId { get; }
    }
}
