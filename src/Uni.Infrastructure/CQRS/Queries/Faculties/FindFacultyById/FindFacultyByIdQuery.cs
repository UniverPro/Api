using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Faculties.FindFacultyById
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
