namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReservationBuilder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Connections", "Reservation_ReservationId", "dbo.Reservations");
            DropIndex("dbo.Connections", new[] { "Reservation_ReservationId" });
            AddColumn("dbo.Reservations", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reservations", "AccountId");
            AddForeignKey("dbo.Reservations", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
            DropColumn("dbo.Connections", "Reservation_ReservationId");
            DropColumn("dbo.Reservations", "totalPrice");
            DropColumn("dbo.Reservations", "totalTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "totalTime", c => c.Double(nullable: false));
            AddColumn("dbo.Reservations", "totalPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Connections", "Reservation_ReservationId", c => c.Int());
            DropForeignKey("dbo.Reservations", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Reservations", new[] { "AccountId" });
            DropColumn("dbo.Reservations", "AccountId");
            CreateIndex("dbo.Connections", "Reservation_ReservationId");
            AddForeignKey("dbo.Connections", "Reservation_ReservationId", "dbo.Reservations", "ReservationId");
        }
    }
}
