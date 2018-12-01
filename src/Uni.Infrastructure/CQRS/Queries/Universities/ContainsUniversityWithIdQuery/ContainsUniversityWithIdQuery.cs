using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.ContainsUniversityWithIdQuery
{
    public class ContainsUniversityWithIdQuery : IQuery<bool>
    {
        public ContainsUniversityWithIdQuery(int universityId)
        {
            UniversityId = universityId;
        }

        public int UniversityId { get; }
    }
}
