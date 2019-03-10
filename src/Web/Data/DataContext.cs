using System.Data.Entity;
using Web.Data.Models;

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

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>()
                .HasMany(f => f.UnreadNotifications)
                .WithMany()
                .Map(s => s.ToTable("SubscribersUnreadNotifications").MapLeftKey("SubscriberId").MapRightKey("NotificationId"));

            modelBuilder.Entity<Subscriber>()
                .HasMany(f => f.Sensors)
                .WithMany()
                .Map(s => s.ToTable("SubscribersSensors").MapLeftKey("SubscriberId").MapRightKey("SensorId"));
            base.OnModelCreating(modelBuilder);
        }
    }
}