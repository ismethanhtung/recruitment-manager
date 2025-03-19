using InsternShip.Data.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InsternShip.Data.Entities
{
    public partial class RecruitmentDB : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        private readonly string connStr = "";
        public RecruitmentDB()
        {

        }
        public RecruitmentDB(DbContextOptions<RecruitmentDB> options, IConfiguration configuration)
        : base(options)
        {
            connStr = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationStatusUpdate> ApplicationStatusUpdates { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<QuestionBank> QuestionBanks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<BlackList> BlackLists { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<RecruiterJobPost> RecruiterJobPosts { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<EventParticipation> EventParticipations { get; set; }
        public DbSet<InterviewSession> InterviewNotes { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Interviewer> Interviewers { get; set; }
        public DbSet<RecruiterEventPost> RecruiterEventPosts { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleClaims> RoleClaims { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connStr);
            } 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }
            modelBuilder.Entity<UserAccount>().Property(i => i.Id).HasColumnName("UserId");
            modelBuilder.Entity<Roles>().Property(i => i.Id).HasColumnName("RoleId");

            modelBuilder.Seed();
            OnModelCreatingPartial(modelBuilder);
            modelBuilder.Entity<Question>()
            .Property(b => b.QuestionId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Test>()
            .Property(b => b.TestId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<QuestionBank>()
            .Property(b => b.QuestionBankId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Application>()
            .Property(b => b.ApplicationId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ApplicationStatusUpdate>()
            .Property(b => b.ApplicationStatusUpdateId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ApplicationStatus>()
            .Property(b => b.ApplicationStatusId)
            .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Event>()
              .Property(b => b.EventId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<EventParticipation>()
              .Property(b => b.ParticipationId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Interview>()
              .Property(b => b.InterviewId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Interviewer>()
              .Property(b => b.InterviewerId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<InterviewSession>()
              .Property(b => b.InterviewSessionId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Job>()
              .Property(b => b.JobId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Event>()
              .Property(b => b.EventId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<EventParticipation>()
              .Property(b => b.ParticipationId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Recruiter>()
              .Property(b => b.RecruiterId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RecruiterEventPost>()
              .Property(b => b.EventPostId)
              .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<RecruiterJobPost>()
              .Property(b => b.JobPostId)
              .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Application>()
            .Property(a => a.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Candidate>()
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Event>()
            .Property(e => e.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Interview>()
            .Property(i => i.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Interviewer>()
            .Property(i => i.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<InterviewSession>()
            .Property(i => i.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Test>()
            .Property(t => t.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<Recruiter>()
            .Property(r => r.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<RecruiterJobPost>()
            .Property(r => r.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<RecruiterEventPost>()
            .Property(r => r.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<UserAccount>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);
            modelBuilder.Entity<UserInfo>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}