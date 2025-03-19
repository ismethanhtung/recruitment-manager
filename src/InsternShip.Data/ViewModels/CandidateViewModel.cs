namespace InsternShip.Data.ViewModels
{
    public class CandidateViewModel
    {
        public Guid UserId { get; set; }
        public Guid CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Language { get; set; }
        public string? Skillsets { get; set; }
        public string? Avatar { get; set; }
        public string? CV { get; set; }
    }
    public class CandidateListViewModel
    {
        //public int QuestionId { get; set; }
        //new List<TestViewModel>();
        public int? TotalCount { get; set; }
        public virtual ICollection<CandidateViewModel>? CandidateList { get; set; }
    }
}
