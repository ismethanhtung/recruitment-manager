namespace InsternShip.Data.ViewModels
{
    public class InterviewSessionViewModel
    {
        public Guid SessionId { get; set; }
        public Guid InterviewId { get; set; }
        public Guid InterviewerId { get; set; }
        public Guid? TestId { get; set; }
        public string? CandidateName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public int GivenScore { get; set; }
        public string? Note { get; set; }
        public bool? IsInterviewDeleted { get; set; }

    }
    public class InterviewSessionBaseViewModel
    {
        public Guid SessionId { get; set; }
        public Guid InterviewId { get; set; }
        public Guid InterviewerId { get; set; }
        public Guid? TestId { get; set; }
        public int GivenScore { get; set; }
        public string? Note { get; set; }

    }
    public class InterviewSessionListInterviewer
    {
        public virtual ICollection<InterviewerViewModel>? Interviewers { get; set; }
    }
    public class InterviewSessionListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<InterviewSessionViewModel>? InterviewSessionList { get; set; }
    }

    public class InterviewSessionBaseListViewModel
    {
        public InterviewSessionBaseViewModel? CurrentSessionInfo { get; set; }
        public AllInfoUser? InterviewerInfo { get; set; }
        public TestViewModel? TestInfo { get; set; }
    }

    public class InterviewSessionInfoViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<InterviewSessionBaseListViewModel>? ListInfoSessions { get; set; }
    }

    public class InterviewSessionDetailViewModel
    {
        public InterviewInfoViewModel? InterviewInfo { get; set; }
        public AllInfoUser? CandidateInfo { get; set; }
        public InterviewSessionInfoViewModel? InterviewSessions { get; set; }
    }
}
