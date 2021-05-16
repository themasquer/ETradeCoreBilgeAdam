using Business.Models.Filters;
using Business.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcWebUI.Models
{
    public class ProductsReportAjaxIndexViewModel
    {
        public ProductsReportAjaxIndexViewModel()
        {
            PageNumber = 1;
        }

        public List<ProductsReportModel> ProductsReport { get; set; }
        public ProductsReportFilterModel ProductsFilter { get; set; }
        public SelectList Categories { get; set; }

        public int PageNumber { get; set; }
        public SelectList Pages { get; set; }
    }
}
