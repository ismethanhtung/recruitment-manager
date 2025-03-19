namespace InsternShip.Data.ViewModels
{
    public class RecruiterPostedViewModel
    {
        public Guid? RecruiterId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
