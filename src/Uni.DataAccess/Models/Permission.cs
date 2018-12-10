using System.Collections.Generic;

namespace Uni.DataAccess.Models
{
    public class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}