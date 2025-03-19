namespace InsternShip.Data.Model
{
    public class InterviewerModel
    {
        public Guid InterviewerId { get; set; }
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class InterviewerUpdateModel
    {
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
    }
    public class CreateInterviewerModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
    }
}
