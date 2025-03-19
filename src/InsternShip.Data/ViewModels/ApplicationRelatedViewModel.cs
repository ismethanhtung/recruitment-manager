namespace InsternShip.Data.ViewModels
{
    public class ApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid JobPostId { get; set; }
        public string? CandidateName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public string? AppliedPosition { get; set; }
        public DateTime ApplyDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
    public class ApplicationBaseViewModel
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid JobPostId { get; set; }
        public DateTime ApplyDate { get; set; }
    }
    public class ApplicationStatusViewModel
    {
        public Guid ApplicationStatusId { get; set; }
        public string? Description { get; set; }
    }
    public class ApplicationStatusUpdateViewModel
    {
        public Guid StatusId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime LatestUpdate { get; set; }
    }
    public class ApplicationStatusUpdateCreateViewModel
    {
        public Guid ApplicationId { get; set; }
    }
    public class ApplicationListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<ApplicationViewModel>? ApplicationList { get; set; }
    }

    public class AppliedInfoViewModel
    {
        public virtual ApplicationViewModel? Application { get; set; }
        public virtual RecruiterJobPostViewModel? JobPost { get; set; }

        //public virtual JobViewModel? Job { get; set; }

    }
    public class AppliedListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<AppliedInfoViewModel>? AppliedList { get; set; }
    }

    public class ApplicationInfoViewModel
    {
        public ApplicationBaseViewModel? CurrentAppliInfo { get; set; }
        public RecruiterJobPostViewModel? JobPostInfo { get; set; }
    }
}
