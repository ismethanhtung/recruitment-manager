namespace InsternShip.Data.Model
{
    public class EventModel
    {
        public Guid EventId { get; set; }
        //public int RecruiterId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int? MaxCandidate { get; set; }
        //public int? RegisteredCandidate { get; set; }
        public string? Description { get; set; }
        public string? Poster { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
    public class CreateEventModel
    {
        //public Guid EventId { get; set; }
        //public int RecruiterId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int? MaxCandidate { get; set; }
        //public int? RegisteredCandidate { get; set; }
        public string? Description { get; set; }
        public string? Poster { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public DateTime? PostDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
    public class EventUpdateModel
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int? MaxCandidate { get; set; }
        public string? Description { get; set; }
        public string? Poster { get; set; }
        public bool? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
}


