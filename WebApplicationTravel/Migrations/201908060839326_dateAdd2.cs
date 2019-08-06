namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateAdd2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Departure", c => c.String());
            DropColumn("dbo.Reservations", "DepDate");
            DropColumn("dbo.Reservations", "retDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "retDate", c => c.String());
            AddColumn("dbo.Reservations", "DepDate", c => c.String());
            DropColumn("dbo.Reservations", "Departure");
        }
    }
}
