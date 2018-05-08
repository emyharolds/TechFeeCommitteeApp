namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommitteeMemberReview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Vote = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(nullable: false, maxLength: 500),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        RequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Request", t => t.RequestId)
                .Index(t => t.User_Id)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        InitialPassword = c.String(),
                        IsPasswordChanged = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemDescription = c.String(nullable: false, maxLength: 50),
                        ItemCost = c.Decimal(nullable: false, storeType: "money"),
                        ItemUsage = c.String(nullable: false, maxLength: 500),
                        Justification = c.String(nullable: false, maxLength: 500),
                        NoOfStudentsImpacted = c.Int(nullable: false),
                        DateOfSubmission = c.DateTime(nullable: false, storeType: "date"),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DepartmentId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        IsApprovedByDepartmentChair = c.Boolean(),
                        DepartmentChairComment = c.String(maxLength: 500),
                        RankByDept = c.String(maxLength: 50),
                        CommitteeChairReview = c.String(maxLength: 50),
                        CommitteeChairComment = c.String(maxLength: 500),
                        IsApprovedByDean = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Session", t => t.SessionId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.UserId)
                .Index(t => t.DepartmentId)
                .Index(t => t.SessionId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Semester = c.String(nullable: false, maxLength: 50, unicode: false),
                        Year = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentClassRequest",
                c => new
                    {
                        StudentClassId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => new { t.StudentClassId, t.RequestId })
                .ForeignKey("dbo.Request", t => t.RequestId)
                .ForeignKey("dbo.StudentClass", t => t.StudentClassId)
                .Index(t => t.StudentClassId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.StudentClass",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Period",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CommitteeMemberReview", "RequestId", "dbo.Request");
            DropForeignKey("dbo.CommitteeMemberReview", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentClassRequest", "StudentClassId", "dbo.StudentClass");
            DropForeignKey("dbo.StudentClass", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.StudentClassRequest", "RequestId", "dbo.Request");
            DropForeignKey("dbo.Request", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Request", "SessionId", "dbo.Session");
            DropForeignKey("dbo.Request", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Request", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.StudentClass", new[] { "DepartmentId" });
            DropIndex("dbo.StudentClassRequest", new[] { "RequestId" });
            DropIndex("dbo.StudentClassRequest", new[] { "StudentClassId" });
            DropIndex("dbo.Request", new[] { "StatusId" });
            DropIndex("dbo.Request", new[] { "SessionId" });
            DropIndex("dbo.Request", new[] { "DepartmentId" });
            DropIndex("dbo.Request", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentId" });
            DropIndex("dbo.CommitteeMemberReview", new[] { "RequestId" });
            DropIndex("dbo.CommitteeMemberReview", new[] { "User_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Period");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.StudentClass");
            DropTable("dbo.StudentClassRequest");
            DropTable("dbo.Status");
            DropTable("dbo.Session");
            DropTable("dbo.Request");
            DropTable("dbo.Department");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CommitteeMemberReview");
        }
    }
}
