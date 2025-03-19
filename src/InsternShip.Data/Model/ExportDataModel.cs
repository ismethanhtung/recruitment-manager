namespace InsternShip.Data.Model
{
    public class CandidateReportModel
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid JobPostId { get; set; }
        public string?  CandidateName { get; set; }
        public string? Description { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Language { get; set; }

        public string? Skillsets { get; set; }
        public string? Status { get; set; }
        public bool IsApplicationDeleted { get; set; }
    }

    public class RecruitmentReportModel
    {
        public Guid JobPostId { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public string? Location { get; set; }
        public string? Benefit { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int Quantity { get; set; }
        public int NumOfRegistration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsJobPostDeleted { get; set; }
    }

    public class EventReportModel
    {
        public Guid EventPostId { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public int? NumOfRegistration { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Method { get; set; }
        public bool? IsEventDeleted { get; set; }
    }

    public class InterviewReportModel
    {
        public Guid InterviewId { get; set; }
        public Guid JobPostId { get; set; }
        public int NumberOfInterviewer { get; set; }
        public string? JobPosition { get; set; }

        public string? CandidateName { get; set; }
        public string? CandidateEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Method { get; set; }

    }
}
