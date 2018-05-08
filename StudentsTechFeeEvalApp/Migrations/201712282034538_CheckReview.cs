namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommitteeMemberReview", "IsReviewes", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommitteeMemberReview", "IsReviewes");
        }
    }
}
