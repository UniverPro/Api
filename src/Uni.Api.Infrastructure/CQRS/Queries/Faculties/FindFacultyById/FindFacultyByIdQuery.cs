using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Faculties.FindFacultyById
{
    public class FindFacultyByIdQuery : IQuery<Faculty>
    {
        public FindFacultyByIdQuery(int facultyId)
        {
            FacultyId = facultyId;
        }

        public int FacultyId { get; }
    }
}
