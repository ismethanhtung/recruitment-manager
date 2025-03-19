namespace InsternShip.Data.ViewModels
{
    public class RecruiterViewModel
    {
        public Guid UserId { get; set; }
        public Guid RecruiterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        public string? Avatar { get; set; }
    }
    public class RecruiterListViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<RecruiterViewModel>? RecruiterList { get; set;}
    }
}
