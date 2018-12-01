using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.CheckUniversityExists
{
    public class CheckUniversityExistsQuery : IQuery<bool>
    {
        public CheckUniversityExistsQuery(int universityId)
        {
            UniversityId = universityId;
        }

        public int UniversityId { get; }
    }
}
