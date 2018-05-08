namespace StudentsTechFeeEvalApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Model_Classes;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StudentsTechFeeEvalApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(StudentsTechFeeEvalApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roleNames = { "CommitteeChairman", "CommitteeMember", "Faculty", "DepartmentChair", "Dean" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                if (!RoleManager.RoleExists(roleName))
                {
                    roleResult = RoleManager.Create(new IdentityRole(roleName));
                }
            }

            context.Departments.AddOrUpdate(
                d => d.Name,
                new Department { Name = "Computer Information Systems" },
                new Department { Name = "Business Administration" },
                new Department { Name = "Criminal Justice" },
                new Department { Name = "Accountancy" },
                new Department { Name = "Aviation Safety" }
                );

            context.Periods.AddOrUpdate(
                d => d.Name,
                new Period { Name = "Setup", IsActive = true },
                new Period { Name = "Submission", IsActive = false },
                new Period { Name = "Departmental Review", IsActive = false },
                new Period { Name = "Committee Review", IsActive = false },
                new Period { Name = "Committee Chair Comments", IsActive = false },
                new Period { Name = "Dean's Review", IsActive = false }
                );

            context.Sessions.AddOrUpdate(
                s => s.Year,
                new Session { Semester = "Fall", Year = new DateTime(2018, 01, 01) }
                );

            context.Status.AddOrUpdate(
               s => s.StatusName,
               new Status { StatusName = "Awaiting Department Chair Approval" },
               new Status { StatusName = "Awaiting Committee Approval" },
               new Status { StatusName = "Awaiting Dean's Approval" },
               new Status { StatusName = "Approved" },
               new Status { StatusName = "Denied" }
               );

            context.StudentClasses.AddOrUpdate(
                s => s.Id,
                new StudentClass { Id = 4660, Name = "CIS 4660 Software Engineering" },
                new StudentClass { Id = 3665, Name = "BSA 3665 Business Informatics" },
                new StudentClass { Id = 2000, Name = "CRJ 2000 Introduction to Civil Justice" },
                new StudentClass { Id = 4550, Name = "ACC 4550 Accounting for Tax" },
                new StudentClass { Id = 4990, Name = "AVS 4990 Aviation Mechanics" }
                );

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var PasswordHash = new PasswordHasher();
            if (!context.Users.Any(u => u.UserName == "700123456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700123456",
                    DepartmentId = 1,
                    FirstName = "Harold",
                    MiddleName = "N",
                    LastName = "Okafor",
                    Email = "harold@ucmo.edu",
                    InitialPassword = "Okafor0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "Okafor0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "CommitteeChairman");
            }

            //create a Faculty User Account
            if (!context.Users.Any(u => u.UserName == "700223456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700223456",
                    DepartmentId = 2,
                    FirstName = "James",
                    MiddleName = "O",
                    LastName = "Conner",
                    Email = "james@ucmo.edu",
                    InitialPassword = "James0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "James0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "DepartmentChair");
            }

            //create another Faculty User Account
            if (!context.Users.Any(u => u.UserName == "700323456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700323456",
                    DepartmentId = 3,
                    FirstName = "David",
                    MiddleName = "J",
                    LastName = "Locke",
                    Email = "david@ucmo.edu",
                    InitialPassword = "David0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "David0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "Faculty");
            }

            //create a Dean User Account
            if (!context.Users.Any(u => u.UserName == "700423456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700423456",
                    DepartmentId = 3,
                    FirstName = "John",
                    MiddleName = "Oliver",
                    LastName = "Barnes",
                    Email = "john@ucmo.edu",
                    InitialPassword = "John0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "John0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "Dean");
            }

            //create a DepartmentChair User Account
            if (!context.Users.Any(u => u.UserName == "700523456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700523456",
                    DepartmentId = 4,
                    FirstName = "Chris",
                    MiddleName = "O",
                    LastName = "Wallace",
                    Email = "chris@ucmo.edu",
                    InitialPassword = "Chris0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "Chris0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "DepartmentChair");
            }

            //create a CommitteeMember User Account
            if (!context.Users.Any(u => u.UserName == "700523457"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700523457",
                    DepartmentId = 2,
                    FirstName = "May",
                    MiddleName = "O",
                    LastName = "Weather",
                    Email = "may@ucmo.edu",
                    InitialPassword = "Maid0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "Maid0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "CommitteeMember");
            }

            //department chair for the criminal justice department
            if (!context.Users.Any(u => u.UserName == "700523856"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700523856",
                    DepartmentId = 2,
                    FirstName = "Christian",
                    MiddleName = "Candy",
                    LastName = "Jack",
                    Email = "chrisr@ucmo.edu",
                    InitialPassword = "Chris0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "Christian0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "DepartmentChair");
            }

            if (!context.Users.Any(u => u.UserName == "700623456"))
            {
                var user = new ApplicationUser
                {
                    UserName = "700623456",
                    DepartmentId = 4,
                    FirstName = "Jerry",
                    MiddleName = "N",
                    LastName = "Sanders",
                    Email = "sanders@ucmo.edu",
                    InitialPassword = "Jerry0.",
                    IsPasswordChanged = false,
                    PasswordHash = PasswordHash.HashPassword("123456")
                };
                IdentityResult result = UserManager.Create(user, "Jerry0.");

                if (result.Succeeded == false) { throw new Exception(result.Errors.First()); }
                UserManager.AddToRole(user.Id, "Faculty");
            }

        }
    }
    
}
