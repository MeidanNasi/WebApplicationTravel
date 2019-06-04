namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Connections", "FlightDuration", c => c.Double());
            AlterColumn("dbo.Connections", "CarDuration", c => c.Double());
            AlterColumn("dbo.Connections", "FlightPrice", c => c.Double());
            AlterColumn("dbo.Connections", "CarPrice", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Connections", "CarPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Connections", "FlightPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Connections", "CarDuration", c => c.Double(nullable: false));
            AlterColumn("dbo.Connections", "FlightDuration", c => c.Double(nullable: false));
        }
    }
}
