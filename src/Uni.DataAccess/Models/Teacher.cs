using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Teacher : Person
    {
        public Teacher()
        {
            Subjects = new HashSet<Subject>();
        }

        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }
}