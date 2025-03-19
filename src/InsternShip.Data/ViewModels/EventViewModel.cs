namespace InsternShip.Data.ViewModels
{
    public class EventViewModel 
    {
        public Guid EventId { get; set; }
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
    public class EventListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<EventViewModel>? EventList { get; set; }
    }
}

