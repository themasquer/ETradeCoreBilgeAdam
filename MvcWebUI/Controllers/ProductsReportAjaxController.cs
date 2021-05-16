using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using Business.Models.Filters;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Models;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsReportAjaxController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsReportAjaxController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        //public IActionResult Index()
        public IActionResult Index(int? categoryId)
        {
            //var productsFilter = new ProductsReportFilterModel();
            var productsFilter = new ProductsReportFilterModel()
            {
                CategoryId = categoryId
            };

            var page = new PageModel()
            {
                RecordsPerPageCount = AppSettings.RecordsPerPageCount
            };
            var result = _productService.GetProductsReport(productsFilter, page);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            var productsReport = result.Data;

            #region Paging
            double recordsCount = page.RecordsCount; // filtrelenmiş veya filtrelenmemiş toplam kayıt sayısı
            double recordsPerPageCount = page.RecordsPerPageCount; // sayfa başına kayıt sayısı
            double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount); // toplam sayfa sayısı
            List<SelectListItem> pageSelectListItems = new List<SelectListItem>();
            if (totalPageCount == 0)
            {
                pageSelectListItems.Add(new SelectListItem()
                {
                    Value = "1",
                    Text = "1"
                });
            }
            else
            {
                for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                {
                    pageSelectListItems.Add(new SelectListItem()
                    {
                        Value = pageNumber.ToString(),
                        Text = pageNumber.ToString()
                    });
                }
            }
            #endregion

            var viewModel = new ProductsReportAjaxIndexViewModel()
            {
                ProductsReport = productsReport,
                ProductsFilter = productsFilter,
                Pages = new SelectList(pageSelectListItems, "Value", "Text"),

                // Categories artık view component üzerinden kullanıldığı için tekrar doldurmaya gerek yok
                //Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(ProductsReportAjaxIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var page = new PageModel()
                {
                    PageNumber = viewModel.PageNumber,
                    RecordsPerPageCount = AppSettings.RecordsPerPageCount
                };

                // Sıralama
                #region Ordering
                var order = new OrderModel()
                {
                    Expression = viewModel.OrderByExpression,
                    DirectionAscending = viewModel.OrderByDirectionAscending
                };
                #endregion

                var result = _productService.GetProductsReport(viewModel.ProductsFilter, page, order);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                viewModel.ProductsReport = result.Data;

                #region Paging
                double recordsCount = page.RecordsCount; // filtrelenmiş veya filtrelenmemiş toplam kayıt sayısı
                double recordsPerPageCount = page.RecordsPerPageCount; // sayfa başına kayıt sayısı
                double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount); // toplam sayfa sayısı
                List<SelectListItem> pageSelectListItems = new List<SelectListItem>();
                if (totalPageCount == 0)
                {
                    pageSelectListItems.Add(new SelectListItem()
                    {
                        Value = "1",
                        Text = "1"
                    });
                }
                else
                {
                    for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                    {
                        pageSelectListItems.Add(new SelectListItem()
                        {
                            Value = pageNumber.ToString(),
                            Text = pageNumber.ToString()
                        });
                    }
                }
                #endregion

                viewModel.Pages = new SelectList(pageSelectListItems, "Value", "Text", viewModel.PageNumber);
            }

            // Categories artık view component üzerinden kullanıldığı için tekrar doldurmaya gerek yok
            //viewModel.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name", viewModel.ProductsFilter.CategoryId);

            return PartialView("_ProductsReport", viewModel);
        }
    }
}
