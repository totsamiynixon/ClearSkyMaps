using DAL.Migrations;
using Entity;
using Entity.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataContext : DbContext
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
        public DbSet<Reading> Readings { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>()
               .Property(c => c.TrackingKey)
               .HasMaxLength(450)
               .IsRequired()
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Sensor_Key")));
        }
    }
}
