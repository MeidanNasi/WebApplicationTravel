namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeResInAccoint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "Account_AccountId", "dbo.Accounts");
            DropIndex("dbo.Reservations", new[] { "Account_AccountId" });
            DropColumn("dbo.Reservations", "Account_AccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "Account_AccountId", c => c.Int());
            CreateIndex("dbo.Reservations", "Account_AccountId");
            AddForeignKey("dbo.Reservations", "Account_AccountId", "dbo.Accounts", "AccountId");
        }
    }
}
