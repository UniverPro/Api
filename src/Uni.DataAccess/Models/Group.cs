using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Group
    {
        public Group()
        {
            Students = new HashSet<Student>();
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public int FacultyId { get; set; }
        public string Name { get; set; }
        public int CourseNumber { get; set; }

        public Faculty Faculty { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}
