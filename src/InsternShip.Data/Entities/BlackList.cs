using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("BlackList")]
    public class BlackList
    {
        [Key]
        [Column("BlackListId")]
        public Guid BlackListId { get; set; }

        public Guid? UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual UserAccount? UserAccount { get; set; }

        public string? Reason { get; set; }
        public int? Duration { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool? IsCurrentlyActive { get; set; }
    }

}
