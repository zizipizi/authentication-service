using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Data.Models.Entities
{
    [Table("access_token")]
    public class AccessTokenEntity
    {
        public int Id { get; set; }

        public int RefreshId { get; set; }

        public string Token { get; set; }

    }
}
