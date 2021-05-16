using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories.Bases
{
    public abstract class ProductRepositoryBase : RepositoryBase<Product>
    {
        protected ProductRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
