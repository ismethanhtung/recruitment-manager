namespace InsternShip.Data.ViewModels
{
    public class RecruiterEventPostViewModel
    {
        public Guid EventPostId { get; set; }
        public Guid RecruiterId { get; set; }

        // infor Recruiter
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // infor Event
        public string? EventName { get; set; }
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

    }

    public class RecruiterEventPostListViewModel
    {
        public int TotalCount { get; set; }
        public ICollection<RecruiterEventPostViewModel>? EventPostList { get; set; }
    }
}
