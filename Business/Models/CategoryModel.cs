using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppCore.Records.Bases;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        //[StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [StringLength(400, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Description { get; set; }

        [DisplayName("Product Count")]
        public int ProductCount { get; set; }
    }
}
