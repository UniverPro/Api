using System.Collections.Generic;

namespace Uni.Api.DataAccess.Models
{
    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
    }
}