namespace InsternShip.Data.Model
{
    public class ApplicationModel
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid JobPostId { get; set; }
    }
    public class ApplicationCreateModel
    {
        public Guid CandidateId { get; set; }
        public Guid JobPostId { get; set; }
    }
    public class ApplicationStatusModel
    {
        public Guid ApplicationStatusId { get; set; }
        public string? Description { get; set; }
    }
   public class ApplicationStatusCreateModel
    {
        public string? Description { get; set; }
    }
    public class ApplicationStatusUpdateModel
    {
        public Guid ApplicationStatusUpdateId { get; set; }
        public Guid StatusId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime LatestUpdate { get; set; }
    }

    public class ApplicationStatusUpdateCreateModel
    {
        public Guid StatusId { get; set; }
        public Guid ApplicationId { get; set; }
    }

}