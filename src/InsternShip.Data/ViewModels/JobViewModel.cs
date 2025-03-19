namespace InsternShip.Data.ViewModels
{
    public class JobViewModel
    {
        // Info key
        public Guid JobId { get; set; }
        // Info job
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
    }

    public class JobListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<JobViewModel>? JobList { get; set; }
    }
}
