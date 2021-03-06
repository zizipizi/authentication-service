﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("UserRole")]
    public class UserRolesEntity
    {
        [Column("role_id")]
        [Required, ForeignKey(nameof(RoleEn))]
        public long RoleId { get; set; }

        [Column("user_id")]
        [Required, ForeignKey(nameof(UserEn))]
        public long UserId { get; set; }

        public virtual RoleEntity RoleEn { get; set; }

        public virtual UserEntity UserEn { get; set; }
    }
}
