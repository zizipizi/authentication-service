using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Access_token")]
    public class AccessTokenEntity
    {
        [Key]
        [Column("id", Order = 1)]
        public long Id { get; set; }

        [Column("user_id")]
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }
        
        [Column("expired")]
        public DateTime Exprired { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("ip_adress")]
        public string IpAdress { get; set; }

        public UserEntity User { get; set; }

        //[Column("refresh_token_jti", Order = 2)]
        public RefreshTokenEntity RefreshToken { get; set; }
    }
}
