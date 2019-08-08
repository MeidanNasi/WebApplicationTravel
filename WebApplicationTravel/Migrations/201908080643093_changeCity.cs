namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cities", "Coordinate_PointId", "dbo.Points");
            DropIndex("dbo.Cities", new[] { "Coordinate_PointId" });
            AddColumn("dbo.Cities", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Cities", "Longitude", c => c.Double(nullable: false));
            DropColumn("dbo.Cities", "Coordinate_PointId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "Coordinate_PointId", c => c.Int());
            DropColumn("dbo.Cities", "Longitude");
            DropColumn("dbo.Cities", "Latitude");
            CreateIndex("dbo.Cities", "Coordinate_PointId");
            AddForeignKey("dbo.Cities", "Coordinate_PointId", "dbo.Points", "PointId");
        }
    }
}
