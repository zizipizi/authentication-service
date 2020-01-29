using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Host.Models
{
    public class BodyTokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
