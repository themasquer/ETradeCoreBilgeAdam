using AppCore.Business.Models.Results;
using Business.Models;
using Business.Models.Reports;
using Business.Services.Bases;
using DataAccess.EntityFramework.Repositories.Bases;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppCore.Business.Models.Paging;
using Business.Models.Filters;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepositoryBase _productRepository;
        private readonly CategoryRepositoryBase _categoryRepository;

        public ProductService(ProductRepositoryBase productRepository, CategoryRepositoryBase categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public Result Add(ProductModel model)
        {
            try
            {
                //var product = _productRepository.GetEntityQuery().SingleOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper().Trim());
                //var product = _productRepository.GetEntityQuery(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()).SingleOrDefault();
                //if (product != null)
                //    return new ErrorResult("Product with the same name exists!");

                if (_productRepository.EntityQuery().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Product with the same name exists!");

                double unitPrice;
                //unitPrice = Convert.ToDouble(model.UnitPriceText.Trim().Replace(",", "."), new CultureInfo("en"));
                //unitPrice = Convert.ToDouble(model.UnitPriceText.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                //if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out unitPrice))
                if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                    return new ErrorResult("Unit price must be a decimal number!");

                model.UnitPrice = unitPrice;
                model.ExpirationDate = null;
                if (!string.IsNullOrWhiteSpace(model.ExpirationDateText))
                    model.ExpirationDate = DateTime.Parse(model.ExpirationDateText, new CultureInfo("en"));
                var entity = new Product()
                {
                    CategoryId = model.CategoryId,

                    //Description = model.Description == null ? null : model.Description.Trim(),
                    Description = model.Description?.Trim(),

                    ExpirationDate = model.ExpirationDate,
                    Name = model.Name.Trim(),
                    StockAmount = model.StockAmount,
                    UnitPrice = model.UnitPrice,

                    ImageFileName = model.ImageFileName
                };
                _productRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _productRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }

        public IQueryable<ProductModel> Query()
        {
            var query = _productRepository.EntityQuery("Category").OrderBy(p => p.Name).Select(p => new ProductModel()
            {
                Id = p.Id,
                Guid = p.Guid,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                UnitPriceText = p.UnitPrice.ToString(new CultureInfo("en")),
                StockAmount = p.StockAmount,
                ExpirationDate = p.ExpirationDate,
                ExpirationDateText = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("yyyy/MM/dd", new CultureInfo("en")) : "",
                CategoryId = p.CategoryId,
                Category = new CategoryModel()
                {
                    Id = p.Category.Id,
                    Guid = p.Category.Guid,
                    Name = p.Category.Name,
                    Description = p.Category.Description
                },

                ImageFileName = p.ImageFileName
            });
            return query;
        }

        public Result Update(ProductModel model)
        {
            try
            {
                //var product = _productRepository.GetEntityQuery().SingleOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id);
                //var product = _productRepository.GetEntityQuery(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id).SingleOrDefault();
                //if (product != null)
                //    return new ErrorResult("Product with the same name exists!");

                if (_productRepository.EntityQuery().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id))
                    return new ErrorResult("Product with the same name exists!");

                double unitPrice;
                //unitPrice = Convert.ToDouble(model.UnitPriceText.Trim().Replace(",", "."), new CultureInfo("en"));
                //unitPrice = Convert.ToDouble(model.UnitPriceText.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                //if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out unitPrice))
                if (!double.TryParse(model.UnitPriceText.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                    return new ErrorResult("Unit price must be a decimal number!");

                model.UnitPrice = unitPrice;
                model.ExpirationDate = null;
                if (!string.IsNullOrWhiteSpace(model.ExpirationDateText))
                    model.ExpirationDate = DateTime.Parse(model.ExpirationDateText, new CultureInfo("en"));
                var entity = _productRepository.EntityQuery(p => p.Id == model.Id).SingleOrDefault();
                entity.CategoryId = model.CategoryId;
                //entity.Description = model.Description == null ? null : model.Description.Trim();
                entity.Description = model.Description?.Trim();
                entity.ExpirationDate = model.ExpirationDate;
                entity.Name = model.Name.Trim();
                entity.StockAmount = model.StockAmount;
                entity.UnitPrice = model.UnitPrice;

                entity.ImageFileName = model.ImageFileName;

                _productRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        //[Obsolete("Bu methodun daha yeni bir versiyonu bulunmaktadır.")] 
        // obsolete: kullanıldığı yerde kullanıldığı yapının daha yeni bir versiyonu olduğunu ve bu yeni versiyonun kullanılmasının gerektiğini belirtir. 
        public Result<List<ProductsReportModel>> GetProductsReport(ProductsReportFilterModel filter, PageModel page = null)
        {
            try
            {
                #region Query
                var productQuery = _productRepository.EntityQuery();
                var categoryQuery = _categoryRepository.EntityQuery();

                //var query = from p in productQuery
                //            join c in categoryQuery
                //                on p.CategoryId equals c.Id
                //            orderby c.Name, p.Name // *1
                //            select new ProductsReportModel()
                //            {
                //                CategoryDescription = c.Description,
                //                CategoryName = c.Name,
                //                ExpirationDateText = p.ExpirationDate.HasValue
                //                    ? p.ExpirationDate.Value.ToString("MM/dd/yyyy", new CultureInfo("en"))
                //                    : "",
                //                ProductDescription = p.Description,
                //                ProductName = p.Name,
                //                StockAmount = p.StockAmount,
                //                //UnitPriceText = p.UnitPrice.ToString("C2", new CultureInfo("en")), // *2
                //                UnitPriceText = "$" + p.UnitPrice.ToString(new CultureInfo("en")), // *2
                //                CategoryId = c.Id
                //            };
                var query = productQuery.Join(categoryQuery,
                    p => p.CategoryId,
                    c => c.Id,
                    (p, c) => new ProductsReportModel()
                    {
                        CategoryDescription = c.Description,
                        CategoryName = c.Name,
                        ExpirationDateText = p.ExpirationDate.HasValue
                            ? p.ExpirationDate.Value.ToString("MM/dd/yyyy", new CultureInfo("en"))
                            : "",
                        ProductDescription = p.Description,
                        ProductName = p.Name,
                        StockAmount = p.StockAmount,
                        //UnitPriceText = p.UnitPrice.ToString("C2", new CultureInfo("en")), // *2
                        UnitPriceText = "$" + p.UnitPrice.ToString(new CultureInfo("en")), // *2
                        CategoryId = c.Id,
                        UnitPrice = p.UnitPrice,
                        ExpirationDate = p.ExpirationDate
                    });
                #endregion

                #region Query First Order
                query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.ProductName); // *1
                #endregion

                #region Query Filter
                if (filter.CategoryId.HasValue)
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    //query = query.Where(q => q.ProductName.Equals(filter.ProductName.Trim(), StringComparison.OrdinalIgnoreCase));
                    //query = query.Where(q => q.ProductName.Contains(filter.ProductName.Trim(), StringComparison.OrdinalIgnoreCase));
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceBeginText))
                {
                    double unitPriceBegin = Convert.ToDouble(filter.UnitPriceBeginText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice >= unitPriceBegin);
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceEndText))
                {
                    double unitPriceEnd = Convert.ToDouble(filter.UnitPriceEndText.Replace(",", "."),
                        CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice <= unitPriceEnd);
                }
                if (filter.StockAmountBegin != null)
                    query = query.Where(q => q.StockAmount >= filter.StockAmountBegin.Value);
                if (filter.StockAmountEnd != null)
                    query = query.Where(q => q.StockAmount <= filter.StockAmountEnd.Value);
                if (!string.IsNullOrWhiteSpace(filter.ExpirationDateBeginText)) // 05/09/2021
                {
                    //string day, month, year;
                    //day = filter.ExpirationDateBeginText.Split('/')[1];
                    //month = filter.ExpirationDateBeginText.Split('/')[0];
                    //year = filter.ExpirationDateBeginText.Split('/')[2];
                    //DateTime expirationDateBegin = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                    DateTime expirationDateBegin = DateTime.Parse(filter.ExpirationDateBeginText, new CultureInfo("en"));

                    query = query.Where(q => q.ExpirationDate >= expirationDateBegin);
                }
                if (!string.IsNullOrWhiteSpace(filter.ExpirationDateEndText))
                {
                    DateTime expirationDateEnd = DateTime.Parse(filter.ExpirationDateEndText, new CultureInfo("en"));
                    query = query.Where(q => q.ExpirationDate <= expirationDateEnd);
                }
                #endregion

                #region Query Paging
                if (page != null)
                {
                    page.RecordsCount = query.Count();
                    int skip = (page.PageNumber - 1) * page.RecordsPerPageCount;
                    int take = page.RecordsPerPageCount;
                    query = query.Skip(skip).Take(take); // *1 Önce mutlaka herhangi bir özelliğe veya özelliklere göre sıralama yapılmalı!
                }
                #endregion

                return new SuccessResult<List<ProductsReportModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<ProductsReportModel>>(exc);
            }
        }
    }
}
