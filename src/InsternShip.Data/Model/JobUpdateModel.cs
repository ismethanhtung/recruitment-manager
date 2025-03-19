namespace InsternShip.Data.Model
{
    public class JobUpdateModel
    {
        // Info key
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Level { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Requirement { get; set; }
        public string? Benefit { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool JobStatus { get; set; } = true;
    }
}