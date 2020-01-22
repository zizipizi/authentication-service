using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Key, Required]
        public long Id { get; set; }

        [Required, MaxLength(128)]
        public string Login { get; set; }

        [Required, MaxLength(1024)]
        public string Password { get; set; }

        public DateTime Created { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<UserRolesEntity> Roles { get; set; }
    }
}
