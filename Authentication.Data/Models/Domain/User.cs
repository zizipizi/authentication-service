using System.Collections;
using System.Collections.Generic;
using Authentication.Data.Models.Entities;

namespace Authentication.Data.Models.Domain
{
    public class User
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }
    }
}
