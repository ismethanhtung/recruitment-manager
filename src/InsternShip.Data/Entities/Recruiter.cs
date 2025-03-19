using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Recruiter")]
    public class Recruiter
    {
        [Key]
        [Column("RecruiterId")]
        public Guid RecruiterId { get; set; }

        // FK qua UserAccount
        [Required]
        public Guid? UserId { get; set; }

        [Column("Description")]
        public string? Description { get; set; }
        [Column("UrlContact")]
        public string? UrlContact { get; set; }

        [ForeignKey("UserId")]
        public virtual UserAccount? UserAccount { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }

    [Table("RecruiterJobPost")]
    public class RecruiterJobPost
    {
        [Key]
        [Column("JobPostId")]
        public Guid JobPostId { get; set; }

        // FK qua Recruiter
        //[Required]
        public Guid? RecruiterId { get; set; }
        [ForeignKey("RecruiterId")]
        public virtual Recruiter? Recruiter { get; set; }
        // FK qua Job
        [Required]
        public Guid JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }
        public virtual ICollection<Application>? Applications { get; set; }

        [Column("IsDeleted")]
        [Required]
        public bool IsDeleted { get; set; }

    }


    [Table("RecruiterEventPost")]
    public class RecruiterEventPost
    {
        [Key]
        [Column("EventPostId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EventPostId { get; set; }

        // FK qua Recruiter
        //[Required]
        public Guid? RecruiterId { get; set; }
        [ForeignKey("RecruiterId")]
        public virtual Recruiter? Recruiter { get; set; }
        // FK qua Event
        [Required]
        public Guid EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event? Event { get; set; }

        [Column("IsDeleted")]
        [Required]
        public bool IsDeleted { get; set; }

    }
}
