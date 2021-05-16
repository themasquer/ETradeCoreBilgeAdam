using AppCore.Business.Models.Results;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        //private readonly ETradeContext _context;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        //public ProductsController(ETradeContext context)
        //{
        //    _context = context;
        //}
        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var eTradeContext = _context.Products.Include(p => p.Category);
        //    return View(await eTradeContext.ToListAsync());
        //}
        [AllowAnonymous]
        //public IActionResult Index()
        public IActionResult Index(string message = null, int? id = null)
        {
            var query = _productService.Query();
            var model = query.ToList();
            ViewData["ProductsMessage"] = message;
            ViewBag.ProductId = id;
            return View(model);

            // Index aksiyonunun hata aldığındaki davranışını görebilmek için:
            //throw new Exception("Test Exception!");
        }

        // GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return View("NotFound");
            }

            var query = _productService.Query();

            var model = query.SingleOrDefault(p => p.Id == id.Value);

            if (model == null)
            {
                //return NotFound();
                return View("NotFound");
            }

            return View(model);
        }

        // GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var query = _categoryService.Query();
            ViewBag.Categories = new SelectList(query.ToList(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([Bind("Name,Description,UnitPrice,StockAmount,ExpirationDate,CategoryId,Id,Guid")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}
        //public IActionResult Create(ProductModel product)
        public IActionResult Create(ProductModel product, IFormFile image) // .NET Core: IFormFile, .NET Framework: HttpPostedFileBase
        {
            Result productResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {

                string fileName = null; 
                string fileExtension = null; 
                string filePath = null; // sunucuda dosyayı kaydedeceğim yol
                bool saveFile = false; // flag
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName; // asusrog.jpg
                    fileExtension = Path.GetExtension(fileName); // .jpg
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false; // flag
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); // bytes
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    saveFile = true;
                }

                if (saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension; // x345f-dert5-gfds2-6hjkl.jpg

                    filePath = Path.Combine("wwwroot", "files", "products", fileName); // .NET Core
                    // .NET Framework: Server.MapPath("~/wwwroot/files/products/x345f-dert5-gfds2-6hjkl.jpg")

                    // wwwroot/files/products/x345f-dert5-gfds2-6hjkl.jpg (sanal yol: virtual path)
                    // D:\BilgeAdam\ETradeCoreBilgeAdam\MvcWebUI\wwwroot\files\products\x345f-dert5-gfds2-6hjkl.jpg (fiziksel yol: physical, absolute path)
                }
                product.ImageFileName = fileName;

                productResult = _productService.Add(product);
                if (productResult.Status == ResultStatus.Exception) // exception
                {
                    throw new Exception(productResult.Message);
                }
                if (productResult.Status == ResultStatus.Success) // success
                {
                    if (saveFile)
                    {
                        // .NET Core:
                        using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            image.CopyTo(fileStream);
                        }

                        // .NET Framework:
                        // image.SaveAs(filePath); // image: HttpPostedFileBase
                    }

                    //return RedirectToAction("Index");
                    return RedirectToAction(nameof(Index)); // nameof(Index) = "Index"
                }

                // error
                //ViewBag.Message = productResult.Message;
                ModelState.AddModelError("", productResult.Message);

                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            // validation error
            categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View("NotFound");

            var productQuery = _productService.Query();
            var product = productQuery.SingleOrDefault(p => p.Id == id.Value);
            if (product == null)
                return View("NotFound");

            var categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
            
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int id, [Bind("Name,Description,UnitPrice,StockAmount,ExpirationDate,CategoryId,Id,Guid")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}
        //public IActionResult Edit(ProductModel product)
        public IActionResult Edit(ProductModel product, IFormFile image)
        {
            Result productResult;
            IQueryable<CategoryModel> categoryQuery;
            if (ModelState.IsValid)
            {

                string fileName = null;
                string fileExtension = null;
                string filePath = null;
                bool saveFile = false;
                if (image != null && image.Length > 0)
                {
                    fileName = image.FileName;
                    fileExtension = Path.GetExtension(fileName);
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false;
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "The image extension is not allowed, the accepted image extensions are " + AppSettings.AcceptedImageExtensions);
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }
                    
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2); // bytes
                    if (image.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "The image size is not allowed, the accepted image size must be maximum " + AppSettings.AcceptedImageMaximumLength + " MB");
                        categoryQuery = _categoryService.Query();
                        ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                        return View(product);
                    }

                    saveFile = true;
                }

                var existingProduct = _productService.Query().SingleOrDefault(p => p.Id == product.Id);
                if (string.IsNullOrWhiteSpace(existingProduct.ImageFileName) && saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension;
                }
                else // existingProduct.ImageFileName = x345f-dert5-gfds2-6hjkl.jpg, fileExtension = png
                {
                    int periodIndex = existingProduct.ImageFileName.IndexOf("."); // 23
                    fileName = existingProduct.ImageFileName.Substring(0, periodIndex + 1); // x345f-dert5-gfds2-6hjkl.
                    fileName = fileName + fileExtension; // x345f-dert5-gfds2-6hjkl.png
                }

                product.ImageFileName = fileName;

                productResult = _productService.Update(product);
                if (productResult.Status == ResultStatus.Exception) // exception
                {
                    throw new Exception(productResult.Message);
                }
                if (productResult.Status == ResultStatus.Success) // success
                {
                    if (saveFile)
                    {
                        filePath = Path.Combine("wwwroot", "files", "products", fileName);
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                    }

                    //return RedirectToAction("Index");
                    return RedirectToAction(nameof(Index)); // nameof(Index) = "Index"
                }

                // error
                //ViewBag.Message = productResult.Message;
                ModelState.AddModelError("", productResult.Message);

                categoryQuery = _categoryService.Query();
                ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            // validation error
            categoryQuery = _categoryService.Query();
            ViewBag.Categories = new SelectList(categoryQuery.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Login", "Accounts");

            if (!id.HasValue)
                return View("NotFound");

            var existingProduct = _productService.Query().SingleOrDefault(p => p.Id == id.Value);

            var result = _productService.Delete(id.Value);
            if (result.Status == ResultStatus.Success)
            {
                if (!string.IsNullOrWhiteSpace(existingProduct.ImageFileName))
                {
                    string filePath = Path.Combine("wwwroot", "files", "products", existingProduct.ImageFileName);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                return RedirectToAction(nameof(Index));
            }
            throw new Exception(result.Message);
        }

        public IActionResult DeleteProductImage(int? id)
        {
            if (id == null)
                return View("NotFound");

            var existingProduct = _productService.Query().SingleOrDefault(p => p.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(existingProduct.ImageFileName))
            {
                string filePath = Path.Combine("wwwroot", "files", "products", existingProduct.ImageFileName);
                existingProduct.ImageFileName = null;
                var result = _productService.Update(existingProduct);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            return View(nameof(Details), existingProduct);
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
