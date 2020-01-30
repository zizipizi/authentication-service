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
        [Column("id")]
        public long Id { get; set; }

        [Column("user_name")]
        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Column("login")]
        [Required, MaxLength(128)]
        public string Login { get; set; }

        [Column("password")]
        [Required, MaxLength(1024)]
        public string Password { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public virtual ICollection<UserRolesEntity> Roles { get; set; }
    }
}
