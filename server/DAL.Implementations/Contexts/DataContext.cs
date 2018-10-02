using Contracts;
using DAL.Intarfaces;
using Domain.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations.Contexts
{
    public class DataContext : DbContext, IDataContext
    {
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;


        public DataContext() : base("DefaultConnection")
        {
            Database.CommandTimeout = 180;

            // the terrible hack. Don't remove this line
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            if (_databaseInitialized)
            {
                return;
            }
            lock (Lock)
            {
                if (!_databaseInitialized)
                {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
                    _databaseInitialized = true;
                }
            }
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
