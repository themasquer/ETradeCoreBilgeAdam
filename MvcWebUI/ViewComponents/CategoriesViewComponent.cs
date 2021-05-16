using System.Collections.Generic;
using System.Linq;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MvcWebUI.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke()
        {
            List<CategoryModel> categories = _categoryService.Query().ToList();
            return View(categories);
        }
    }
}
