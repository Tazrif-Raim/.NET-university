namespace Zero_Hunger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZeroHungerV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accesses",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Benefactors",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false),
                        AccessName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accesses", t => t.AccessName)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.AccessName);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                        ExpireTime = c.DateTime(nullable: false),
                        CompleteTime = c.DateTime(),
                        Amount = c.String(nullable: false),
                        StatusName = c.String(nullable: false, maxLength: 128),
                        AssignedTo = c.Guid(),
                        BenefactorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefactors", t => t.BenefactorId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.AssignedTo)
                .ForeignKey("dbo.Status", t => t.StatusName, cascadeDelete: true)
                .Index(t => t.StatusName)
                .Index(t => t.AssignedTo)
                .Index(t => t.BenefactorId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Foods", "StatusName", "dbo.Status");
            DropForeignKey("dbo.Foods", "AssignedTo", "dbo.Employees");
            DropForeignKey("dbo.Foods", "BenefactorId", "dbo.Benefactors");
            DropForeignKey("dbo.Benefactors", "Id", "dbo.Users");
            DropForeignKey("dbo.Users", "AccessName", "dbo.Accesses");
            DropIndex("dbo.Foods", new[] { "BenefactorId" });
            DropIndex("dbo.Foods", new[] { "AssignedTo" });
            DropIndex("dbo.Foods", new[] { "StatusName" });
            DropIndex("dbo.Users", new[] { "AccessName" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Benefactors", new[] { "Id" });
            DropTable("dbo.Status");
            DropTable("dbo.Foods");
            DropTable("dbo.Employees");
            DropTable("dbo.Users");
            DropTable("dbo.Benefactors");
            DropTable("dbo.Accesses");
        }
    }
}
