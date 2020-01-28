using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Refresh_token")]
    public class RefreshTokenEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("expired")]
        public DateTime Expired { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("refresh_token_jti")]
        public string Jti { get; set; }

    }
}
