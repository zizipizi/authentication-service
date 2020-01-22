using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("Access_token")]
    public class AccessTokenEntity
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(RefreshToken))]
        public long RefreshId { get; set; }

        public string Token { get; set; }

        public DateTime Expriry { get; set; }

        public virtual RefreshTokenEntity RefreshToken { get; set; }
    }
}
