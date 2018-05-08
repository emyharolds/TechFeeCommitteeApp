namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDeptFromClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentClass", "DepartmentId", "dbo.Department");
            DropIndex("dbo.StudentClass", new[] { "DepartmentId" });
            DropColumn("dbo.StudentClass", "DepartmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentClass", "DepartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.StudentClass", "DepartmentId");
            AddForeignKey("dbo.StudentClass", "DepartmentId", "dbo.Department", "Id");
        }
    }
}
