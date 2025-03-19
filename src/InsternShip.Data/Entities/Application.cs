using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Application")]
    public class Application
    {
        [Key]
        [Column("ApplicationId")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ApplicationId { get; set; }

        //FK qua Candidate
        [Required]
        public Guid CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate? Candidate { get; set; }

        [Required]
        public Guid JobPostId { get; set; }
        [ForeignKey("JobPostId")]
        public virtual RecruiterJobPost? RecruiterJobPosts { get; set; }

        [Column("ApplyDate")]
        public DateTime ApplyDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationStatusUpdate>? ApplicationStatusUpdates { get; set; }
    }

    [Table("ApplicationStatusUpdate")]
    public class ApplicationStatusUpdate
    {
        [Key]
        [Column("ApplicationStatusUpdateId")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ApplicationStatusUpdateId { get; set; }


        [Required]
        public Guid StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual ApplicationStatus? Status { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }

        [Column("LatestUpdate")]
        public DateTime LatestUpdate { get; set; }
    }

    [Table("ApplicationStatus")]
    public class ApplicationStatus
    {
        [Key]
        [Column("ApplicationStatusId")]
        public Guid ApplicationStatusId { get; set; }

        [Column("Description")]
        public string? Description { get; set; }
        [Required]
        //public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationStatusUpdate>? ApplicationStatusUpdates { get; set; }
    }

    
}
