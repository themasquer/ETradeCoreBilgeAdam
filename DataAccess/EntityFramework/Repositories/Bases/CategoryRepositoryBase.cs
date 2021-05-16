using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories.Bases
{
    public abstract class CategoryRepositoryBase : RepositoryBase<Category>
    {
        protected CategoryRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
