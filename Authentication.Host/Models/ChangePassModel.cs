using System.ComponentModel.DataAnnotations;

namespace Authentication.Host.Models
{
    public class ChangePassModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
