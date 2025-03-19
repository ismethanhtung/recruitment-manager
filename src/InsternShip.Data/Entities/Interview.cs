using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Interview")]
    public class Interview
    {
        [Key]
        [Column("InterviewId")]
        public Guid InterviewId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Guid? ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }
        public ICollection<InterviewSession>? Sessions { get; set; }
        public Interview() { }

    }
    [Table("Interviewer")]
    public class Interviewer
    {

        [Key]
        public Guid InterviewerId { get; set; }

        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserAccount? UserAccount { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        public ICollection<InterviewSession>? Sessions { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public Interviewer() { }
    }
    [Table("InterviewSession")]
    public class InterviewSession
    {
        [Key]
        public Guid InterviewSessionId { get; set; }

        //public bool? Pass { get; set; }
        public int GivenScore { get; set; }
        public string? Note { get; set; }

        public Guid? InterviewId { get; set; }
        [ForeignKey("InterviewId")]
        public virtual Interview? Interview { get; set; }

        public Guid? InterviewerId { get; set; }
        [ForeignKey("InterviewerId")]
        public virtual Interviewer? Interviewer { get; set; }

        public Guid? TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Test? Test { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        //public InterviewSession() { }
    }
}
