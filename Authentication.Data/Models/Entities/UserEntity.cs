﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("users")]
    public class UserEntity
    {
        public long Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public DateTime Created { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}