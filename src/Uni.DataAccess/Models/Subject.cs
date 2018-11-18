namespace Uni.DataAccess.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; }

        public Group Group { get; set; }
        public Teacher Teacher { get; set; }
        public Schedule Schedule { get; set; }
    }
}
