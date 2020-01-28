using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Data.Models.Entities
{
    [Table("UserRole")]
    public class UserRolesEntity
    {
        [Key]
        public long Id { get; set; }
        [Required, ForeignKey(nameof(RoleEn))]
        public long RoleId { get; set; }

        [Required, ForeignKey(nameof(UserEn))]
        public long UserId { get; set; }

        public virtual RoleEntity RoleEn { get; set; }

        public virtual UserEntity UserEn { get; set; }

    }
}
