namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NavigationPropertyAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Readings", "Sensor_Id", "dbo.Sensors");
            DropIndex("dbo.Readings", new[] { "Sensor_Id" });
            RenameColumn(table: "dbo.Readings", name: "Sensor_Id", newName: "SensorId");
            AlterColumn("dbo.Readings", "SensorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Readings", "SensorId");
            AddForeignKey("dbo.Readings", "SensorId", "dbo.Sensors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Readings", "SensorId", "dbo.Sensors");
            DropIndex("dbo.Readings", new[] { "SensorId" });
            AlterColumn("dbo.Readings", "SensorId", c => c.Int());
            RenameColumn(table: "dbo.Readings", name: "SensorId", newName: "Sensor_Id");
            CreateIndex("dbo.Readings", "Sensor_Id");
            AddForeignKey("dbo.Readings", "Sensor_Id", "dbo.Sensors", "Id");
        }
    }
}
