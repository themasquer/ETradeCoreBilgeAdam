using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppCore.Records.Bases;

namespace Business.Models
{
    public class CountryModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Country Name")]
        public string Name { get; set; }
    }
}
