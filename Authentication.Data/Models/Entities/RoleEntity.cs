using System.Collections;
using System.Collections.Generic;

namespace Authentication.Data.Models.Entities
{
    public class RoleEntity
    {
        public long Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleEntity> UserRoles { get; set; }

    }
}
