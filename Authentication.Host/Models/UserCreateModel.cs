using System.ComponentModel.DataAnnotations;

namespace Authentication.Host.Models
{
    public class UserCreateModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
