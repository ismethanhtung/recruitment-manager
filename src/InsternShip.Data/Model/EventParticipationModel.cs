namespace InsternShip.Data.Model
{
    public class EventParticipationModel
    {
        public Guid ParticipationId { get; set; }
        public Guid CandidateId { get; set; }
        //public Guid? CandidateId { get; set; }
        public Guid EventPostId { get; set; }
        public bool? Status { get; set; }
    }
    public class EventParticipationCreateModel
    {
        //public Guid ParticipationId { get; set; }
        public Guid CandidateId { get; set; }
        //public Guid? CandidateId { get; set; }
        public Guid EventPostId { get; set; }
    }
    
}


