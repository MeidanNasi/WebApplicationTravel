namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carAndFlightAvailabilty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Connections", "CarAvailabilty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Connections", "FlightAvailabilty", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Connections", "FlightAvailabilty");
            DropColumn("dbo.Connections", "CarAvailabilty");
        }
    }
}
