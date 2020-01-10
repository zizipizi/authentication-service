using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Host.Models
{
    public class ChangePassModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
