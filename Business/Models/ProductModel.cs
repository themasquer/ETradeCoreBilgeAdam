using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AppCore.Records.Bases;

namespace Business.Models
{
    public class ProductModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        //[StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [MinLength(2, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "{0} is required!")]
        public double UnitPrice { get; set; }

        //[DisplayName("Unit Price")]
        //public string UnitPriceText => UnitPrice.ToString(new CultureInfo("en"));

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "{0} is required!")]
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")]
        [Required(ErrorMessage = "{0} is required!")]
        //[Range(0, Int32.MaxValue, ErrorMessage = "{0} must be between {1} and {2}!")]
        [Range(0, 9999, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        //[DisplayName("Expiration Date")]
        //public string ExpirationDateText => ExpirationDate.HasValue ? ExpirationDate.Value.ToString("MM/dd/yyyy", new CultureInfo("en")) : "";

        [DisplayName("Expiration Date")]
        public string ExpirationDateText { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        [StringLength(255, ErrorMessage = "{0} must be maximum {1} characters!")] 
        [DisplayName("Image")]
        public string ImageFileName { get; set; }
    }
}
