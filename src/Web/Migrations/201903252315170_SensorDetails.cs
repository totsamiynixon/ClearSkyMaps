namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SensorDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sensors", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sensors", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sensors", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Sensors", "IPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sensors", "IPAddress");
            DropColumn("dbo.Sensors", "Type");
            DropColumn("dbo.Sensors", "IsDeleted");
            DropColumn("dbo.Sensors", "IsVisible");
            DropColumn("dbo.Sensors", "IsActive");
        }
    }
}
