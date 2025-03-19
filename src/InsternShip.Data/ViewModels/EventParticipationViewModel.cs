namespace InsternShip.Data.ViewModels
{
    public class EventParticipationViewModel 
    {
        public Guid ParticipationId { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? EventPostId { get; set; }
        //Candidate
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Language { get; set; }
        public string? Skillsets { get; set; }
        public bool? Status { get; set; }
    }
    public class EventParticipationListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<EventParticipationViewModel>? EventParticipationList { get; set; }
    }

    public class EventPostParticipationViewModel
    {
        public Guid ParticipationId { get; set; }
        public Guid? EventPostId { get; set; }
        // infor Recruiter
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // infor Event
        public string? EventName { get; set; }
        public string? Location { get; set; }
        public int? MaxCandidate { get; set; }
        public string? Description { get; set; }
        public string? Poster { get; set; }
        public bool? Online { get; set; }
        public bool? Approved { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
    }
    public class EventPostParticipationListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<EventPostParticipationViewModel>? EventPostParticipationList { get; set; }
    }
}

