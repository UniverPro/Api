using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Subject
    {
        public Subject()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
