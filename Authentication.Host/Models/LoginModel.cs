﻿using System.ComponentModel.DataAnnotations;

namespace Authentication.Host.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        internal string IpAddress { get; set; } = null;
    }
}
