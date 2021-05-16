using AppCore.Business.Validations;
using System.ComponentModel;

namespace Business.Models.Filters
{
    public class ProductsReportFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Product Name")] 
        public string ProductName { get; set; }

        [DisplayName("Unit Price")]
        [StringDecimal]
        public string UnitPriceBeginText { get; set; }

        [StringDecimal]
        public string UnitPriceEndText { get; set; }

        [DisplayName("Stock Amount")] 
        public int? StockAmountBegin { get; set; }

        public int? StockAmountEnd { get; set; }

        [DisplayName("Expiration Date")] 
        public string ExpirationDateBeginText { get; set; }

        public string ExpirationDateEndText { get; set; }
    }
}
