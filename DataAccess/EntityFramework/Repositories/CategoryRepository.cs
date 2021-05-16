using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{
    public class CategoryRepository : CategoryRepositoryBase
    {
        public CategoryRepository(DbContext db) : base(db)
        {

        }
    }
}
