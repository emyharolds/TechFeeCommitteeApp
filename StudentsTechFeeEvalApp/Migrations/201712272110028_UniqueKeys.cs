namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueKeys : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CommitteeMemberReview", new[] { "User_Id" });
            DropIndex("dbo.CommitteeMemberReview", new[] { "RequestId" });
            CreateIndex("dbo.CommitteeMemberReview", new[] { "User_Id", "RequestId" }, unique: true, name: "IX_FirstAndSecond");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CommitteeMemberReview", "IX_FirstAndSecond");
            CreateIndex("dbo.CommitteeMemberReview", "RequestId");
            CreateIndex("dbo.CommitteeMemberReview", "User_Id");
        }
    }
}
