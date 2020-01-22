using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Role")]
    public class RoleEntity
    {
        [Key]
        public long Id { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserRolesEntity> UserRoles { get; set; }

    }
}
