using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserDetailModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("E-Mail")]
        [EmailAddress(ErrorMessage = "{0} is not valid!")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        public CountryModel Country { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("City")]
        public int CityId { get; set; }

        public CityModel City { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(1000, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Address { get; set; }
    }
}
