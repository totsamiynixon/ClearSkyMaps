namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaticAndPortableSensors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Readings", "StaticSensor_Id", c => c.Int());
            AddColumn("dbo.Sensors", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Sensors", "Latitude", c => c.Double());
            AlterColumn("dbo.Sensors", "Longitude", c => c.Double());
            AlterColumn("dbo.Sensors", "IsVisible", c => c.Boolean());
            CreateIndex("dbo.Readings", "StaticSensor_Id");
            AddForeignKey("dbo.Readings", "StaticSensor_Id", "dbo.Sensors", "Id");
            DropColumn("dbo.Sensors", "TrackingKey");
            DropColumn("dbo.Sensors", "Type");
            DropColumn("dbo.Sensors", "IsMated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sensors", "IsMated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sensors", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Sensors", "TrackingKey", c => c.String());
            DropForeignKey("dbo.Readings", "StaticSensor_Id", "dbo.Sensors");
            DropIndex("dbo.Readings", new[] { "StaticSensor_Id" });
            AlterColumn("dbo.Sensors", "IsVisible", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Sensors", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Sensors", "Latitude", c => c.Double(nullable: false));
            DropColumn("dbo.Sensors", "Discriminator");
            DropColumn("dbo.Readings", "StaticSensor_Id");
        }
    }
}
