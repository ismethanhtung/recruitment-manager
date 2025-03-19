namespace InsternShip.Data.ViewModels
{
    public class InterviewViewModel
    {
        public Guid InterviewId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobPostId { get; set; }
        public string? CandidateName { get; set; }
        public string? JobName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
    }
    public class InterviewDetailViewModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public Guid CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public int NumberOfInterviewer { get; set; }
        public bool IsOnline { get; set; }
        public virtual ICollection<InterviewerViewModel>? Interviewers { get; set; }

    }
    public class InterviewListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<InterviewViewModel>? InterviewList { get; set; }

    }

    public class MyInterviewViewModel
    {
        public Guid InterviewId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public int NumberOfInterviewer { get; set; }

    }

    public class MyInterviewListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<MyInterviewViewModel>? InterviewList { get; set; }

    }
    
    public class InterviewInfoViewModel
    {
        public InterviewViewModel? CurrentInterviewInfo { get; set; }
        public ApplicationInfoViewModel? ApplicationInfo { get; set; }
    }
}
