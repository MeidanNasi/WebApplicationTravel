namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        CountryId = c.Int(nullable: false),
                        FlightPriceKey = c.Double(nullable: false),
                        CarRentalPriceKey = c.Double(nullable: false),
                        Coordinate_X = c.Double(nullable: false),
                        Coordinate_Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionsId = c.Int(nullable: false, identity: true),
                        SourceCityId = c.Int(),
                        DestCityId = c.Int(),
                        FlightDuration = c.Double(nullable: false),
                        CarDuration = c.Double(nullable: false),
                        FlightPrice = c.Double(nullable: false),
                        CarPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ConnectionsId)
                .ForeignKey("dbo.Cities", t => t.DestCityId)
                .ForeignKey("dbo.Cities", t => t.SourceCityId)
                .Index(t => t.SourceCityId)
                .Index(t => t.DestCityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "SourceCityId", "dbo.Cities");
            DropForeignKey("dbo.Connections", "DestCityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropIndex("dbo.Connections", new[] { "DestCityId" });
            DropIndex("dbo.Connections", new[] { "SourceCityId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropTable("dbo.Connections");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
        }
    }
}
