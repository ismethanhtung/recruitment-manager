namespace InsternShip.Data.Model
{
    public class InterviewModel
    {
        public Guid InterviewId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
        public bool IsDeleted { get; set; }
        
    }
    public class CreateInterviewModel
    {
        public Guid ApplicationId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
    }
    public class InterviewUpdateModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }

    }
}
