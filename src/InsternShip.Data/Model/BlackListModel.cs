namespace InsternShip.Data.Model
{
    public class BlackListModel
    {
        public Guid BlackListId { get; set; }
        public Guid UserId { get; set; }
        public string? Reason { get; set; }
        public int? Duration { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool? IsCurrentlyActive { get; set; }
    }
    public class CreateBlackListModel
    {
        public Guid UserId { get; set; }
        public string? Reason { get; set; }
        public int? Duration { get; set; }
    }
    public class BlackListUpdateModel
    {
        public string? Reason { get; set; }
        public int? Duration { get; set; }
    }
}