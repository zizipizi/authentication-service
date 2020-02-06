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
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("expired")]
        public DateTime Expired { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("is_blocked")]
        public bool IsBlocked { get; set; }

        //[Key]
        [Column(name:"token_jti")]
        public string Jti { get; set; }

        public virtual UserEntity User { get; set; }

        [InverseProperty(nameof(AccessTokenEntity.RefreshToken))]
        public virtual ICollection<AccessTokenEntity> AccessToken { get; set; }
    }
}
