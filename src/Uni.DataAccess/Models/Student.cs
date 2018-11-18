namespace Uni.DataAccess.Models
{
    public class Student : Person
    {
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}