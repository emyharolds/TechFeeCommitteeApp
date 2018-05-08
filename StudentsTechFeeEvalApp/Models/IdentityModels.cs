using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentsTechFeeEvalApp.Models.Model_Classes;
using StudentsTechFeeEvalApp.ModelConfigurations;

namespace StudentsTechFeeEvalApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CommitteeMemberReviews = new HashSet<CommitteeMemberReview>();
            Requests = new HashSet<Request>();
        }

        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; } // FirstName (length: 50)
       
        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } // MiddleName (length: 50)

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } // LastName (length: 50)

        public string FullName
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }
        public int DepartmentId { get; set; } // DepartmentId

        [Display(Name = "Initial Password")]
        public string InitialPassword { get; set; } // InitialPassword (length: 50)
        public bool IsPasswordChanged { get; set; } // IsPasswordChanged
        public virtual Department Department { get; set; } // FK_User_Department

        public virtual ICollection<CommitteeMemberReview> CommitteeMemberReviews { get; set; } // CommitteeMemberReview.FK_CommitteeMemberReview_Request

        public virtual ICollection<Request> Requests { get; set; } // Request.FK_Request_User



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName.ToString()));
            userIdentity.AddClaim(new Claim("IsPasswordChanged", this.IsPasswordChanged.ToString()));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<CommitteeMemberReview> CommitteeMemberReviews { get; set; } // CommitteeMemberReview
        public DbSet<Department> Departments { get; set; } // Department
        public DbSet<Period> Periods { get; set; } // Period
        public DbSet<Request> Requests { get; set; } // Request
        public DbSet<Session> Sessions { get; set; } // Session
        public DbSet<Status> Status { get; set; } // Status

        [Display(Name = "Classes Supported: ")]
        public DbSet<StudentClass> StudentClasses { get; set; } // StudentClass
        //public DbSet<StudentClassRequest> StudentClassRequests { get; set; } // StudentClassRequest

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new CommitteeMemberReviewConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new PeriodConfiguration());
            modelBuilder.Configurations.Add(new RequestConfiguration());
            modelBuilder.Configurations.Add(new SessionConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            modelBuilder.Configurations.Add(new StudentClassConfiguration());
            //modelBuilder.Configurations.Add(new StudentClassRequestConfiguration());
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            //modelBuilder.Configurations.Add(new ApplicationUserConfiguration(schema));
            modelBuilder.Configurations.Add(new CommitteeMemberReviewConfiguration(schema));
            modelBuilder.Configurations.Add(new DepartmentConfiguration(schema));
            modelBuilder.Configurations.Add(new PeriodConfiguration(schema));
            modelBuilder.Configurations.Add(new RequestConfiguration(schema));
            modelBuilder.Configurations.Add(new SessionConfiguration(schema));
            modelBuilder.Configurations.Add(new StatusConfiguration(schema));
            modelBuilder.Configurations.Add(new StudentClassConfiguration(schema));
            //modelBuilder.Configurations.Add(new StudentClassRequestConfiguration(schema));
            return modelBuilder;
        }
    }
}