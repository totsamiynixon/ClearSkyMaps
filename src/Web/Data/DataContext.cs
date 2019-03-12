using System.Data.Entity;
using Web.Data.Models;
using Web.Helpers;

namespace Web.Data
{
    public class DataContext : DbContext
    {
        private static readonly object Lock = new object();
        private static bool _databaseInitialized;


        public DataContext() : base()
        {
            Database.CommandTimeout = 180;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Database.Connection.ConnectionString = SettingsHelper.ConnectionString;

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

        public virtual DbSet<Reading> Readings { get; set; }

        public virtual DbSet<Sensor> Sensors { get; set; }
    }
}