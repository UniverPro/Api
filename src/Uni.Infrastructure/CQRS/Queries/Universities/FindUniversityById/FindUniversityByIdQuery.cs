using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.FindUniversityById
{
    public class FindUniversityByIdQuery : IQuery<University>
    {
        public FindUniversityByIdQuery(int universityId)
        {
            UniversityId = universityId;
        }

        public int UniversityId { get; }
    }
}
