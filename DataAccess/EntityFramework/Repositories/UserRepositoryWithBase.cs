using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{
    public abstract class UserRepositoryBase : RepositoryBase<User>
    {
        protected UserRepositoryBase(DbContext db) : base(db)
        {
            
        }
    }

    public class UserRepository : UserRepositoryBase
    {
        public UserRepository(DbContext db) : base(db)
        {
            
        }
    }
}
