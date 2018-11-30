using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Students.FindStudentById
{
    public class FindStudentByIdQuery : IQuery<Student>
    {
        public FindStudentByIdQuery(int studentId)
        {
            StudentId = studentId;
        }

        public int StudentId { get; }
    }
}
