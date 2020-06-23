using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Role")]
    public class RoleEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("role")]
        public string Role { get; set; }
       
        [Column("description")]
        public string Description { get; set; }

        public virtual ICollection<UserRolesEntity> UserRoles { get; set; }

    }
}
