using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Test")]
    public class Test
    {
        [Key]
        [Column("TestId")]
        public Guid TestId { get; set; }

        [Column("TotalScore")]
        public int? TotalScore { get; set; }

        [Column("StartTime")]
        public DateTime? StartTime { get; set; }

        [Column("EndTime")]
        public DateTime? EndTime { get; set; }
        public virtual ICollection<QuestionBank>? QuestionBanks { get; set;}
        public virtual InterviewSession? Sessions { get; set;}

        [Required]
        public bool IsDeleted { get; set; }

    }

    [Table("QuestionBank")]
    public class QuestionBank
    {
        [Key]
        [Column("QuestionBankId")]
        public Guid QuestionBankId { get; set; }

        public Guid TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Test? Test { get; set; }

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
    }

    [Table("Question")]
    public class Question
    {
        [Key]
        [Column("QuestionId")]
        public Guid QuestionId { get; set; }

        [Column("Detail")]
        public string? Detail { get; set; }

        [Column("MaxScore")]
        public int? MaxScore { get; set; }

        [Column("Note")]
        public string? Note { get; set; }
        // new
        public string? Tag { get; set; }
        public int? Level { get; set; }
        // old
        public virtual ICollection<QuestionBank>? QuestionBanks { get; set; }
    }
/*    [Table("QuestionTag")]
    public class QuestionTag
    {
        [Key]
        [Column("QuestionTagId")]
        public Guid QuestionTagId { get; set; }

        [Column("Tagname")]
        
        [Required]
        public bool IsDeleted { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
    }*/
}
