namespace InsternShip.Data.ViewModels
{
    public class RecruiterJobPostViewModel
    {
        public Guid JobPostId { get; set; }

        // infor Recruiter
        public Guid RecruiterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // infor Job
        public string? Level { get; set; }
        public string? TypeName { get; set; }
        public string? JobName { get; set; }
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

        //public virtual JobViewModel? Job { get; set; }

    }

    public class RecruiterJobPostListViewModel
    {
        public int TotalCount { get; set; }
        public ICollection<RecruiterJobPostViewModel>? JobPostList { get; set; }
    }
    public class OneRecruiterJobPostListViewModel
    {
        public int TotalCount { get; set; }
        public int CandidateCount { get; set; }
        public int InterviewerCount { get; set; }
        public ICollection<RecruiterJobPostViewModel>? JobPostList { get; set; }
    }

}
