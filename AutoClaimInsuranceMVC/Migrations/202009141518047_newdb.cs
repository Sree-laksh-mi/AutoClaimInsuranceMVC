namespace AutoClaimInsuranceMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        claimId = c.Int(nullable: false, identity: true),
                        insurerId = c.String(nullable: false, maxLength: 128),
                        MailID = c.String(nullable: false),
                        policyNumber = c.Int(nullable: false),
                        dateAndTime = c.DateTime(nullable: false),
                        policeCase = c.String(nullable: false),
                        firNumber = c.String(nullable: false),
                        licenseCopy = c.String(nullable: false),
                        rcCopy = c.String(nullable: false),
                        status = c.String(nullable: false),
                        claimDate = c.DateTime(nullable: false),
                        insurance_policyNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.claimId)
                .ForeignKey("dbo.Insurers", t => t.insurerId, cascadeDelete: true)
                .ForeignKey("dbo.Insurances", t => t.insurance_policyNumber)
                .Index(t => t.insurerId)
                .Index(t => t.insurance_policyNumber);
            
            CreateTable(
                "dbo.Insurances",
                c => new
                    {
                        policyNumber = c.String(nullable: false, maxLength: 128),
                        coverage = c.String(nullable: false),
                        startDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                        vehilceRegistrationNumber = c.String(nullable: false),
                        chassisNumber = c.Int(nullable: false),
                        model = c.String(nullable: false),
                        valueInsured = c.Double(nullable: false),
                        insurerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.policyNumber)
                .ForeignKey("dbo.Insurers", t => t.insurerId, cascadeDelete: true)
                .Index(t => t.insurerId);
            
            CreateTable(
                "dbo.Insurers",
                c => new
                    {
                        insurerId = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        address = c.String(nullable: false),
                        mobileNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.insurerId);
            
            CreateTable(
                "dbo.RegisteredUsers",
                c => new
                    {
                        userId = c.String(nullable: false, maxLength: 128),
                        insurerId = c.String(nullable: false, maxLength: 128),
                        password = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.userId)
                .ForeignKey("dbo.Insurers", t => t.insurerId, cascadeDelete: true)
                .Index(t => t.insurerId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        reportId = c.String(nullable: false, maxLength: 128),
                        officerId = c.String(nullable: false, maxLength: 128),
                        claimId = c.Int(nullable: false),
                        status = c.String(nullable: false),
                        content = c.String(nullable: false),
                        reportDate = c.DateTime(nullable: false),
                        amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.reportId)
                .ForeignKey("dbo.Claims", t => t.claimId, cascadeDelete: true)
                .ForeignKey("dbo.Officers", t => t.officerId, cascadeDelete: true)
                .Index(t => t.officerId)
                .Index(t => t.claimId);
            
            CreateTable(
                "dbo.Officers",
                c => new
                    {
                        officerId = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        password = c.String(nullable: false, maxLength: 255),
                        role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.officerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "officerId", "dbo.Officers");
            DropForeignKey("dbo.Reports", "claimId", "dbo.Claims");
            DropForeignKey("dbo.Claims", "insurance_policyNumber", "dbo.Insurances");
            DropForeignKey("dbo.RegisteredUsers", "insurerId", "dbo.Insurers");
            DropForeignKey("dbo.Insurances", "insurerId", "dbo.Insurers");
            DropForeignKey("dbo.Claims", "insurerId", "dbo.Insurers");
            DropIndex("dbo.Reports", new[] { "claimId" });
            DropIndex("dbo.Reports", new[] { "officerId" });
            DropIndex("dbo.RegisteredUsers", new[] { "insurerId" });
            DropIndex("dbo.Insurances", new[] { "insurerId" });
            DropIndex("dbo.Claims", new[] { "insurance_policyNumber" });
            DropIndex("dbo.Claims", new[] { "insurerId" });
            DropTable("dbo.Officers");
            DropTable("dbo.Reports");
            DropTable("dbo.RegisteredUsers");
            DropTable("dbo.Insurers");
            DropTable("dbo.Insurances");
            DropTable("dbo.Claims");
        }
    }
}
