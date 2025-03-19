namespace InsternShip.Data.ViewModels
{
    public class BlackListViewModel
    {
        public Guid BlacklistId { get; set; }
        public Guid? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public string? RoleName { get; set; }
        public int? Duration { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? Reason { get; set; }
    }
    public class BlackListEntriesViewModel
    {
        public int? TotalCount { get; set; }
        public virtual ICollection<BlackListViewModel>? EntryList { get; set; }
    }
}
