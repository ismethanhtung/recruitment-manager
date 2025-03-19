namespace InsternShip.Data.Model
{
    public class JobModel
    {
        // Info key
        //public Guid JobIndustryId { get; set; }
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
        public bool JobStatus { get; set; } = true;
        //public bool IsDeleted { get; set; }
    }
    public class CreateJobModel
    {
        // Info key
        //public Guid JobIndustryId { get; set; }
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
        //public DateTime CreateDate { get; set; }
        //public DateTime UpdateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool JobStatus { get; set; } = true;
    }
}