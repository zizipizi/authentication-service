using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Refresh_token")]
    public class RefreshTokenEntity
    {
        [Key]
        public long Id { get; set; }

        public string Token { get; set; }

        public DateTime Expiry { get; set; }

        public string Jti { get; set; }

        public bool IsBlocked { get; set;}
    }
}
