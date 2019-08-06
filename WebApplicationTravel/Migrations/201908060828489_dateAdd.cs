namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "DepDate", c => c.String());
            AddColumn("dbo.Reservations", "retDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "retDate");
            DropColumn("dbo.Reservations", "DepDate");
        }
    }
}
