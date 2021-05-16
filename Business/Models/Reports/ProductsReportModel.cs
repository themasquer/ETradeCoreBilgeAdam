using System;
using System.ComponentModel;

namespace Business.Models.Reports
{
    public class ProductsReportModel
    {
        [DisplayName("Product Name")] 
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        [DisplayName("Category Name")] 
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        [DisplayName("Unit Price")] 
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")] 
        public int StockAmount { get; set; }

        [DisplayName("Expiration Date")] 
        public string ExpirationDateText { get; set; }

        public int CategoryId { get; set; }
        public double UnitPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
