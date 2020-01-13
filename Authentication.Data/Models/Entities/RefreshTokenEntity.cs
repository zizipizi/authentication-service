using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("refresh_token")]
    public class RefreshTokenEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Expiry { get; set; }

        private bool IsBlocked { get; set;}
    }
}
