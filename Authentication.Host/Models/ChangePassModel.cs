using System.ComponentModel.DataAnnotations;

namespace Authentication.Host.Models
{
    public class ChangePassModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "min length = 8")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$", ErrorMessage = "Password must meet requirements")]
        public string NewPassword { get; set; }
    }
}
