using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Subjects.FindSubjectById
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
