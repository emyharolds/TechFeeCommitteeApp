namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentClassRequest", "RequestId", "dbo.Request");
            DropForeignKey("dbo.StudentClassRequest", "StudentClassId", "dbo.StudentClass");
            DropIndex("dbo.StudentClassRequest", new[] { "StudentClassId" });
            DropIndex("dbo.StudentClassRequest", new[] { "RequestId" });
            CreateTable(
                "dbo.StudentClassRequests",
                c => new
                    {
                        StudentClass_Id = c.Int(nullable: false),
                        Request_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentClass_Id, t.Request_Id })
                .ForeignKey("dbo.StudentClass", t => t.StudentClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.Request", t => t.Request_Id, cascadeDelete: true)
                .Index(t => t.StudentClass_Id)
                .Index(t => t.Request_Id);
            
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "MiddleName", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            DropTable("dbo.StudentClassRequest");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StudentClassRequest",
                c => new
                    {
                        StudentClassId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => new { t.StudentClassId, t.RequestId });
            
            DropForeignKey("dbo.StudentClassRequests", "Request_Id", "dbo.Request");
            DropForeignKey("dbo.StudentClassRequests", "StudentClass_Id", "dbo.StudentClass");
            DropIndex("dbo.StudentClassRequests", new[] { "Request_Id" });
            DropIndex("dbo.StudentClassRequests", new[] { "StudentClass_Id" });
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "MiddleName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            DropTable("dbo.StudentClassRequests");
            CreateIndex("dbo.StudentClassRequest", "RequestId");
            CreateIndex("dbo.StudentClassRequest", "StudentClassId");
            AddForeignKey("dbo.StudentClassRequest", "StudentClassId", "dbo.StudentClass", "Id");
            AddForeignKey("dbo.StudentClassRequest", "RequestId", "dbo.Request", "Id");
        }
    }
}
