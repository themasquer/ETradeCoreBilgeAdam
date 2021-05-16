using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserRegisterModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "{1} and {0} must be same!")]
        public string ConfirmPassword { get; set; }
    
        public UserDetailModel UserDetail { get; set; }
    }
}
