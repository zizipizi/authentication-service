using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Data.Models.Entities
{
    [Table("UserRoles")]
    public class UserRolesEntity
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(RoleEn))]
        public long RoleID { get; set; }

        [ForeignKey(nameof(UserEn))]
        public long UserID { get; set; }

        public virtual RoleEntity RoleEn { get; set; }

        public virtual UserEntity UserEn { get; set; }
    }
}
