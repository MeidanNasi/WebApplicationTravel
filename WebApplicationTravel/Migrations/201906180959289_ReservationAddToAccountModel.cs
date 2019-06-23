namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationAddToAccountModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationId)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            AddColumn("dbo.Connections", "Reservation_ReservationId", c => c.Int());
            CreateIndex("dbo.Connections", "Reservation_ReservationId");
            AddForeignKey("dbo.Connections", "Reservation_ReservationId", "dbo.Reservations", "ReservationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "Reservation_ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.Reservations", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Connections", new[] { "Reservation_ReservationId" });
            DropIndex("dbo.Reservations", new[] { "AccountId" });
            DropColumn("dbo.Connections", "Reservation_ReservationId");
            DropTable("dbo.Reservations");
        }
    }
}
