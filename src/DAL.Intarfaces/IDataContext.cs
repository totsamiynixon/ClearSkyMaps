using Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Intarfaces
{
    public interface IDataContext : IDisposable
    {
        DbSet<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        Task<int> SaveChangesAsync();
    }
}
