using StudentsTechFeeEvalApp.Models.Model_Classes;

namespace StudentsTechFeeEvalApp.ModelConfigurations
{

    // CommitteeMemberReview
    public class CommitteeMemberReviewConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CommitteeMemberReview>
    {
        public CommitteeMemberReviewConfiguration()
            : this("dbo")
        {
        }

        public CommitteeMemberReviewConfiguration(string schema)
        {
            ToTable("CommitteeMemberReview", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Vote).HasColumnName(@"Vote").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.Comment).HasColumnName(@"Comment").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.UserId).HasColumnName(@"User_Id").HasColumnType("nvarchar").IsRequired().HasMaxLength(128);
            Property(x => x.RequestId).HasColumnName(@"RequestId").HasColumnType("int").IsRequired();

            // Foreign keys
            HasRequired(a => a.ApplicationUser).WithMany(b => b.CommitteeMemberReviews).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_CommitteeMemberReview_Request
            HasRequired(a => a.Request).WithMany(b => b.CommitteeMemberReviews).HasForeignKey(c => c.RequestId).WillCascadeOnDelete(false); // FK_CommitteeMemberReview_User
        }
    }

    // Department
    public class DepartmentConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
            : this("dbo")
        {
        }

        public DepartmentConfiguration(string schema)
        {
            ToTable("Department", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
        }
    }

    // Period
    public class PeriodConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Period>
    {
        public PeriodConfiguration()
            : this("dbo")
        {
        }

        public PeriodConfiguration(string schema)
        {
            ToTable("Period", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.IsActive).HasColumnName(@"IsActive").HasColumnType("bit").IsRequired();
        }
    }

    // Request
    public class RequestConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Request>
    {
        public RequestConfiguration()
            : this("dbo")
        {
        }

        public RequestConfiguration(string schema)
        {
            ToTable("Request", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ItemDescription).HasColumnName(@"ItemDescription").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
            Property(x => x.ItemCost).HasColumnName(@"ItemCost").HasColumnType("money").IsRequired().HasPrecision(19, 4);
            Property(x => x.ItemUsage).HasColumnName(@"ItemUsage").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.Justification).HasColumnName(@"Justification").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            Property(x => x.NoOfStudentsImpacted).HasColumnName(@"NoOfStudentsImpacted").HasColumnType("int").IsRequired();
            Property(x => x.DateOfSubmission).HasColumnName(@"DateOfSubmission").HasColumnType("datetime").IsRequired();
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("nvarchar").IsRequired().HasMaxLength(128);
            Property(x => x.DepartmentId).HasColumnName(@"DepartmentId").HasColumnType("int").IsRequired();
            Property(x => x.SessionId).HasColumnName(@"SessionId").HasColumnType("int").IsRequired();
            Property(x => x.StatusId).HasColumnName(@"StatusId").HasColumnType("int").IsRequired();
            Property(x => x.IsApprovedByDepartmentChair).HasColumnName(@"IsApprovedByDepartmentChair").HasColumnType("bit").IsOptional();
            Property(x => x.DepartmentChairComment).HasColumnName(@"DepartmentChairComment").HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
            Property(x => x.RankByDept).HasColumnName(@"RankByDept").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.CommitteeChairReview).HasColumnName(@"CommitteeChairReview").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.CommitteeChairComment).HasColumnName(@"CommitteeChairComment").HasColumnType("nvarchar").IsOptional().HasMaxLength(500);
            Property(x => x.IsApprovedByDean).HasColumnName(@"IsApprovedByDean").HasColumnType("bit").IsOptional();

            // Foreign keys
            HasRequired(a => a.ApplicationUser).WithMany(b => b.Requests).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_Request_User
            HasRequired(a => a.Department).WithMany(b => b.Requests).HasForeignKey(c => c.DepartmentId).WillCascadeOnDelete(false); // FK_Request_Department
            HasRequired(a => a.Session).WithMany(b => b.Requests).HasForeignKey(c => c.SessionId).WillCascadeOnDelete(false); // FK_Request_Session
            HasRequired(a => a.Status).WithMany(b => b.Requests).HasForeignKey(c => c.StatusId).WillCascadeOnDelete(false); // FK_Request_Status
        }
    }

    // Session
    public class SessionConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Session>
    {
        public SessionConfiguration()
            : this("dbo")
        {
        }

        public SessionConfiguration(string schema)
        {
            ToTable("Session", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Semester).HasColumnName(@"Semester").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.Year).HasColumnName(@"Year").HasColumnType("date").IsRequired();
        }
    }

    // Status
    public class StatusConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Status>
    {
        public StatusConfiguration()
            : this("dbo")
        {
        }

        public StatusConfiguration(string schema)
        {
            ToTable("Status", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.StatusName).HasColumnName(@"StatusName").HasColumnType("nvarchar").IsRequired().HasMaxLength(50);
        }
    }

    // StudentClass
    public class StudentClassConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StudentClass>
    {
        public StudentClassConfiguration()
            : this("dbo")
        {
        }

        public StudentClassConfiguration(string schema)
        {
            ToTable("StudentClass", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            //Property(x => x.DepartmentId).HasColumnName(@"DepartmentId").HasColumnType("int").IsRequired();

            // Foreign keys
            //HasRequired(a => a.Department).WithMany(b => b.StudentClasses).HasForeignKey(c => c.DepartmentId).WillCascadeOnDelete(false); // FK_StudentClass_Department
        }
    }

    // StudentClassRequest
    //public class StudentClassRequestConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<StudentClassRequest>
    //{
    //    public StudentClassRequestConfiguration()
    //        : this("dbo")
    //    {
    //    }

    //    public StudentClassRequestConfiguration(string schema)
    //    {
    //        ToTable("StudentClassRequest", schema);
    //        HasKey(x => new { x.StudentClassId, x.RequestId });

    //        Property(x => x.StudentClassId).HasColumnName(@"StudentClassId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
    //        Property(x => x.RequestId).HasColumnName(@"RequestId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

    //        // Foreign keys
    //        HasRequired(a => a.Request).WithMany(b => b.StudentClassRequests).HasForeignKey(c => c.RequestId).WillCascadeOnDelete(false); // FK_StudentClassRequest_Request
    //        HasRequired(a => a.StudentClass).WithMany(b => b.StudentClassRequests).HasForeignKey(c => c.StudentClassId).WillCascadeOnDelete(false); // FK_StudentClassRequest_StudentClass
    //    }
    //}

}