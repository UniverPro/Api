using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.CheckSubjectExists
{
    public class CheckSubjectExistsQuery : IQuery<bool>
    {
        public CheckSubjectExistsQuery(int subjectId)
        {
            SubjectId = subjectId;
        }

        public int SubjectId { get; }
    }
}
