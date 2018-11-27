using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Teacher : Person
    {
        public Teacher()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
