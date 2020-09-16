 namespace AutoClaimInsuranceMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claims", "reason", c => c.String(nullable: false));
            DropColumn("dbo.Claims", "firNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claims", "firNumber", c => c.String(nullable: false));
            DropColumn("dbo.Claims", "reason");
        }
    }
}
