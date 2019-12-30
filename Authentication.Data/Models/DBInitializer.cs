using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Data.Models
{
    public class DBInitializer
    {
        private AuthContext _ctx;

        public DBInitializer(AuthContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Seed()
        {
            //if (!_ctx.Users.Any())
            //{
            //    _ctx.Add(new UserEntity(
            //    {
            //        Id = _ctx.Users.Max(x => x.Id) + 1,
            //        Login = "Login2",
            //        Password = "Password2",
            //        Created = DateTime.Today,
            //        IsActive = true
            //    }));

            //    await _ctx.SaveChangesAsync();
            //}
        }

    }
}
