namespace Uni.Api.DataAccess.Models
{
    public abstract class Person
    {
        public int Id { get; set; }

        public string AvatarPath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public User User { get; set; }
    }
}
