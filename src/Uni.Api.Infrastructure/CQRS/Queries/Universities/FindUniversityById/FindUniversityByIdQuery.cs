using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Universities.FindUniversityById
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
