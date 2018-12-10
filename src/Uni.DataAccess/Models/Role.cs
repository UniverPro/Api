using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
            RolePermissions = new HashSet<RolePermission>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}