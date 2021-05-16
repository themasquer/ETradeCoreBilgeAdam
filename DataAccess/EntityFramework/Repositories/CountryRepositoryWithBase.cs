using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{
    public abstract class CountryRepositoryBase : RepositoryBase<Country>
    {
        protected CountryRepositoryBase(DbContext db) : base(db)
        {
            
        }
    }

    public class CountryRepository : CountryRepositoryBase
    {
        public CountryRepository(DbContext db) : base(db)
        {
            
        }
    }
}
