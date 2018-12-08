namespace Uni.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Login { get; set; }

        public string Password { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }
    }
}