using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{
    public class ProductRepository : ProductRepositoryBase
    {
        public ProductRepository(DbContext db) : base(db)
        {

        }
    }
}
