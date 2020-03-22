using System.Collections.Generic;

namespace Authentication.Data.Models.Domain
{
    public class User
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string IpAddress { get; set; } = null;

        public IEnumerable<string> Role { get; set; }
    }
}
