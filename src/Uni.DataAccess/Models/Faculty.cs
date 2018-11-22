using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Faculty : ITableObject
    {
        public Faculty()
        {
            Group = new HashSet<Group>();
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }

        public University University { get; set; }
        public ICollection<Group> Group { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}
