using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Host.Models
{
    public class UserCreateModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
