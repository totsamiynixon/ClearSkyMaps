namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SensorsIsMated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sensors", "IsMated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sensors", "IsMated");
        }
    }
}
