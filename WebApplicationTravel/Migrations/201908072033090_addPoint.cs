namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPoint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        PointId = c.Int(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PointId);
            
            AddColumn("dbo.Cities", "Coordinate_PointId", c => c.Int());
            CreateIndex("dbo.Cities", "Coordinate_PointId");
            AddForeignKey("dbo.Cities", "Coordinate_PointId", "dbo.Points", "PointId");
            DropColumn("dbo.Cities", "Coordinate_X");
            DropColumn("dbo.Cities", "Coordinate_Y");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "Coordinate_Y", c => c.Double(nullable: false));
            AddColumn("dbo.Cities", "Coordinate_X", c => c.Double(nullable: false));
            DropForeignKey("dbo.Cities", "Coordinate_PointId", "dbo.Points");
            DropIndex("dbo.Cities", new[] { "Coordinate_PointId" });
            DropColumn("dbo.Cities", "Coordinate_PointId");
            DropTable("dbo.Points");
        }
    }
}
