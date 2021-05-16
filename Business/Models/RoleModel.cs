using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppCore.Records.Bases;

namespace Business.Models
{
    public class RoleModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Role")]
        public string Name { get; set; }

        public List<UserModel> Users { get; set; }
    }
}
