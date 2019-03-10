namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subscribers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscribersSensors",
                c => new
                    {
                        SubscriberId = c.String(nullable: false, maxLength: 128),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubscriberId, t.SensorId })
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId, cascadeDelete: true)
                .ForeignKey("dbo.Sensors", t => t.SensorId, cascadeDelete: true)
                .Index(t => t.SubscriberId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.SubscribersUnreadNotifications",
                c => new
                    {
                        SubscriberId = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubscriberId, t.NotificationId })
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.SubscriberId)
                .Index(t => t.NotificationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscribersUnreadNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.SubscribersUnreadNotifications", "SubscriberId", "dbo.Subscribers");
            DropForeignKey("dbo.SubscribersSensors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SubscribersSensors", "SubscriberId", "dbo.Subscribers");
            DropIndex("dbo.SubscribersUnreadNotifications", new[] { "NotificationId" });
            DropIndex("dbo.SubscribersUnreadNotifications", new[] { "SubscriberId" });
            DropIndex("dbo.SubscribersSensors", new[] { "SensorId" });
            DropIndex("dbo.SubscribersSensors", new[] { "SubscriberId" });
            DropTable("dbo.SubscribersUnreadNotifications");
            DropTable("dbo.SubscribersSensors");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Notifications");
        }
    }
}
