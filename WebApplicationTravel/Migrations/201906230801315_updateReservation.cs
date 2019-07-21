namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "totalPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Reservations", "totalTime", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "totalTime");
            DropColumn("dbo.Reservations", "totalPrice");
        }
    }
}
