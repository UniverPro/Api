using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjectById
{
    public class FindSubjectByIdQuery : IQuery<Subject>
    {
        public FindSubjectByIdQuery(int subjectId)
        {
            SubjectId = subjectId;
        }

        public int SubjectId { get; }
    }
}
