using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : RecordBase
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

        public bool Active { get; set; }

        [DisplayName("Active")]
        public string ActiveText { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "{0} is required!")]
        public int RoleId { get; set; }

        public RoleModel Role { get; set; }

        public int UserDetailId { get; set; }
        public UserDetailModel UserDetail { get; set; }
    }
}
