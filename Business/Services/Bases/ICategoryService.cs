using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;

namespace Business.Services.Bases
{
    public interface ICategoryService : IService<CategoryModel>
    {
        // Async Methodlar
        Task<Result<List<CategoryModel>>> GetCategoriesAsync();
    }
}
