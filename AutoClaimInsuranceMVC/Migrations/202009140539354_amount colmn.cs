namespace AutoClaimInsuranceMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amountcolmn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "amount");
        }
    }
}
