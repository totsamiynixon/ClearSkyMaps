namespace DAL.Implementations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApllicationConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnableEmulation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Readings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CO2 = c.Single(nullable: false),
                        LPG = c.Single(nullable: false),
                        CO = c.Single(nullable: false),
                        CH4 = c.Single(nullable: false),
                        Dust = c.Single(nullable: false),
                        Temp = c.Single(nullable: false),
                        Hum = c.Single(nullable: false),
                        Preassure = c.Single(nullable: false),
                        Created = c.DateTime(nullable: false),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sensors", t => t.SensorId, cascadeDelete: true)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        TrackingKey = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TrackingKey, name: "IX_Sensor_Key");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Readings", "SensorId", "dbo.Sensors");
            DropIndex("dbo.Sensors", "IX_Sensor_Key");
            DropIndex("dbo.Readings", new[] { "SensorId" });
            DropTable("dbo.Sensors");
            DropTable("dbo.Readings");
            DropTable("dbo.ApllicationConfigurations");
        }
    }
}
