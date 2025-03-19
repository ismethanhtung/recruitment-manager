namespace InsternShip.Data.ViewModels
{
    public class InterviewerViewModel
    {
        public Guid UserId { get; set; }
        public Guid InterviewerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        public bool IsDeleted { get; set; }
        public string? Avatar { get; set; }
    }
    public class InterviewerListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<InterviewerViewModel>? InterviewerList { get; set; }

    }
}
