using InsternShip.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Seeding
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            // Seed Status
            builder.Entity<ApplicationStatus>().HasData(
                new { ApplicationStatusId = Guid.NewGuid(), Description = "Unprocessed" },
                new { ApplicationStatusId = Guid.NewGuid(), Description = "Interviewing" },
                new { ApplicationStatusId = Guid.NewGuid(), Description = "Interviewed" },
                new { ApplicationStatusId = Guid.NewGuid(), Description = "Rejected" },
                new { ApplicationStatusId = Guid.NewGuid(), Description = "Approved" });
            // Seed Roles
            List<Roles> roles = new List<Roles>() { };
            Roles admin = new Roles()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN".ToUpper()
            };
            Roles recuiter = new Roles()
            {
                Id = Guid.NewGuid(),
                Name = "Recruiter",
                NormalizedName = "RECRUITER".ToUpper()
            };
            Roles interviewer = new Roles()
            {
                Id = Guid.NewGuid(),
                Name = "Interviewer",
                NormalizedName = "INTERVIEWER".ToUpper()
            };
            Roles candidate = new Roles()
            {
                Id = Guid.NewGuid(),
                Name = "Candidate",
                NormalizedName = "CANDIDATE".ToUpper()
            };
            roles.Add(admin); roles.Add(recuiter); roles.Add(interviewer); roles.Add(candidate);
            builder.Entity<Roles>().HasData(roles);

            // -----------------------------------------------------------------------------

            // Seed Users

            UserInfo uinfo = new UserInfo()
            {
                InfoId = Guid.NewGuid(),
                PhoneNumber = "",
                FirstName = "",
                LastName = "",
                Avatar = "",
                Gender = "",
                DateOfBirth = DateTime.Now,
                Location = "",
                IsDeleted = false

            };

            builder.Entity<UserInfo>().HasData(uinfo);
            var passwordHasher = new PasswordHasher<UserAccount>();
            List<UserAccount> users = new List<UserAccount>()
    {
         // imporant: don't forget NormalizedUserName, NormalizedEmail 
                 new UserAccount {
                    Id = Guid.NewGuid(), // primary key
                    UserName = "hcm23.net03.01@gmail.com",
                    NormalizedUserName = "HCM23.NET03.01@GMAIL.COM",
                    Email = "hcm23.net03.01@gmail.com",
                    NormalizedEmail = "HCM23.NET03.01@GMAIL.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    InfoId = uinfo.InfoId,
                    RegistrationDate = DateTime.Now,
                    IsDeleted = false
                },

    };


            builder.Entity<UserAccount>().HasData(users);

            ///----------------------------------------------------

            // Seed UserRoles


            List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();

            // Add Password For All Users

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "Hcm23@net03");

            userRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = users[0].Id,
                RoleId = admin.Id
            });

            builder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);

        }
    }
}