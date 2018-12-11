using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Students.FindStudentById
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
