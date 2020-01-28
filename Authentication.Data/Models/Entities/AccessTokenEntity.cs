using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Access_token")]
    public class AccessTokenEntity
    {
        
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("refresh_jti")]
        [ForeignKey(nameof(RefreshToken))]
        public string RefreshJti { get; set; }

        [Column("token")]
        public string Token { get; set; }
        
        [Column("expired")]
        public DateTime Exprired { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("ip_adress")]
        public string IpAdress { get; set; }

        public virtual RefreshTokenEntity RefreshToken { get; set; }
    }
}
