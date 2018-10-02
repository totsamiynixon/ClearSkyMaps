using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Contracts;
using DAL.Implementations.Contexts;
using DAL.Intarfaces;
using Effort;

namespace Web.Emulator.Database
{
    public class EmulatorContext : DbContext, IDataContext
    {

        public EmulatorContext(DbConnection connection) : base(connection, true)
        {
        }

        public IDbSet<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(DataContext).Assembly);
        }
    }
}