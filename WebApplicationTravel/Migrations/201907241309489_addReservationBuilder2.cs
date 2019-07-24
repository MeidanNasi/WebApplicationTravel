namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReservationBuilder2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "TheReservation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "TheReservation");
        }
    }
}
