namespace StudentsTechFeeEvalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckReview1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommitteeMemberReview", "IsReviewed", c => c.Boolean(nullable: false));
            DropColumn("dbo.CommitteeMemberReview", "IsReviewes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommitteeMemberReview", "IsReviewes", c => c.Boolean(nullable: false));
            DropColumn("dbo.CommitteeMemberReview", "IsReviewed");
        }
    }
}
