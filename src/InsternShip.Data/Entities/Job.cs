using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    [Table("Job")]
    public class Job
    {
        // Primary key
        [Key]
        [Column("JobId")]
        public Guid JobId { get; set; }
        public string? Type { get; set; }

        public string? Level { get; set; }

        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Requirement { get; set; }
        public string? Benefit { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool JobStatus { get; set; }

        public Job() { }
    }
    /*
    [Table("JobIndustry")]
    public class JobIndustry
    {
        // Primary key
        [Key]
        [Column("JobIndustryId")]
        public Guid JobIndustryId { get; set; }

        // Attribute
        public String? Name { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public JobIndustry() { }
    }

    [Table("JobType")]
    public class JobType
    {
        // Primary key
        [Key]
        [Column("JobTypeId")]
        public Guid JobTypeId { get; set; }

        // Attribute
        public String? Name { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public JobType() { }
    }

    [Table("JobLevel")]
    public class JobLevel
    {
        // Primary key
        [Key]
        [Column("JobLevelId")]
        public Guid JobLevelId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        // Attribute
        public String? Name { get; set; }

        public JobLevel() { }
    }
    */
}
