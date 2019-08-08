namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Point : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Points");
        }
        
        public override void Down()
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
            
        }
    }
}
