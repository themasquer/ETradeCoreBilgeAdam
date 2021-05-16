using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using DataAccess.EntityFramework.Repositories.Bases;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepositoryBase _categoryRepository;
        private readonly ProductRepositoryBase _productRepository;

        public CategoryService(CategoryRepositoryBase categoryRepository, ProductRepositoryBase productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public Result Add(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.EntityQuery().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Category with the same name exists!");
                var entity = new Category()
                {
                    Description = model.Description?.Trim(),
                    Name = model.Name.Trim()
                };
                _categoryRepository.Add(entity);
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
                var category = _categoryRepository.EntityQuery(c => c.Id == id, "Products").SingleOrDefault();
                
                if (category.Products != null && category.Products.Count > 0)
                {
                    // eğer kategoriye bağlı ürünleri de silmek istersek: 
                    //foreach (var product in category.Products)
                    //{
                    //    _productRepository.Delete(product, false);
                    //}

                    return new ErrorResult("Category has products so it can't be deleted!");
                }

                _categoryRepository.Delete(category);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }

        public IQueryable<CategoryModel> Query()
        {
            var query = _categoryRepository.EntityQuery("Products").OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count
            });
            return query;
        }

        public Result Update(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.EntityQuery().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                    return new ErrorResult("Category with the same name exists!");
                var entity = _categoryRepository.EntityQuery(c => c.Id == model.Id).SingleOrDefault();
                entity.Description = model.Description?.Trim();
                entity.Name = model.Name.Trim();
                _categoryRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        // Async Methodlar
        public async Task<Result<List<CategoryModel>>> GetCategoriesAsync()
        {
            try
            {
                List<Category> categoryEntities = await _categoryRepository.Query().OrderBy(c => c.Name).ToListAsync();
                List<CategoryModel> categories = categoryEntities.Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return new SuccessResult<List<CategoryModel>>(categories);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CategoryModel>>(exc);
            }
        }
    }
}
