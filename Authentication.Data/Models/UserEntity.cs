﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Data.Models
{
    [Table("users")]
    public class UserEntity
    {
        public int Id { get; set; }

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
