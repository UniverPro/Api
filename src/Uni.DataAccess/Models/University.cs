using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class University : ITableObject
    {
        public University()
        {
            Faculties = new HashSet<Faculty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }

        public ICollection<Faculty> Faculties { get; set; }
    }
}
