namespace InsternShip.Data.Model
{
    public class InterviewSessionModel
    {
        public Guid InterviewSessionId { get; set; }
        public int GivenScore { get; set; }
        public string? Note { get; set; }
        public Guid? InterviewId { get; set; }
        public Guid? InterviewerId { get; set; }
        public Guid? TestId { get; set; }
    }
    public class CreateInterviewSessionModel
    {
        public string? Note { get; set; }
        public Guid? InterviewId { get; set; }
        public Guid? InterviewerId { get; set; }
        public Guid? TestId { get; set; }
    }
    public class UpdatedInterviewSessionModel
    {
        public int GivenScore { get; set; }
        public string? Note { get; set; }
    }
}
