namespace WebApplicationTravel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeRes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Reservations", new[] { "AccountId" });
            RenameColumn(table: "dbo.Reservations", name: "AccountId", newName: "Account_AccountId");
            AlterColumn("dbo.Reservations", "Account_AccountId", c => c.Int());
            CreateIndex("dbo.Reservations", "Account_AccountId");
            AddForeignKey("dbo.Reservations", "Account_AccountId", "dbo.Accounts", "AccountId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Account_AccountId", "dbo.Accounts");
            DropIndex("dbo.Reservations", new[] { "Account_AccountId" });
            AlterColumn("dbo.Reservations", "Account_AccountId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Reservations", name: "Account_AccountId", newName: "AccountId");
            CreateIndex("dbo.Reservations", "AccountId");
            AddForeignKey("dbo.Reservations", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
        }
    }
}
