using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Candidate")]
    public class Candidate
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CandidateId { get; set; }

        [Required]
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserAccount? UserAccount { get; set; }

        public string? Description { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Language { get; set; }

        public string? Skillsets { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Application>? Applications { get; set; }
        public Candidate() { }
    }
    [Table("CV")]
    public class CV
    {
        [Key]
        public Guid CVId { get; set; }

        [Required]
        public Guid? CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public virtual Candidate? Candidate { get; set; }

        public string? UrlFile { get; set; }
        public string? PublicIdFile { get; set; }
        public CV() { }
    }


}


