using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Event")]
    public class Event
    {
        [Key]
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public string? Name { get; set; }

        //[Column("location", TypeName = "varchar(100)")]
        [Required]
        public string? Location { get; set; }

        //[Column("max_candidate")]
        [Required]
        public int? MaxCandidate { get; set; }

        //[Column("registered_candidate")]
        [Required]
        //public int? RegisteredCandidate { get; set; }

        //[Column("description")]
        public string? Description { get; set; }

        //[Column("poster")]
        public string? Poster { get; set; }
        public bool? Status { get; set; } = false;

        //[Column("start_date")]
        public DateTime? StartDate { get; set; }

        //[Column("end_date")]
        public DateTime? EndDate { get; set; }

        //[Column("post_date")]
        public DateTime? PostDate { get; set; }

        //[Column("deadline_date")]
        public DateTime? DeadlineDate { get; set; }

        [Required]
        //[Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public Event() { }

    }
    [Table("EventParticipation")]
    public class EventParticipation
    {
        [Key]
        [Required]
        public Guid ParticipationId { get; set; }

        [Column(Order = 1)]
        public Guid? CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate? Candidate { get; set; }

        [Column(Order = 2)]
        public Guid? EventPostId { get; set; }
        [ForeignKey("EventPostId")]
        public virtual RecruiterEventPost? RecruiterEventPost { get; set; }

        //[Column("status")]
        public bool? Status { get; set; }

        public EventParticipation() {}
    }

    
}


