namespace InsternShip.Data.Model
{
    public class CandidateModel
    {

        public Guid CandidateId { get; set; }
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Language { get; set; }

        public string? Skillsets { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CreateCandidateModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Language { get; set; }

        public string? Skillsets { get; set; }
    }
    public class CandidateUpdateModel
    {
        public string? Description { get; set; }

        public string? Education { get; set; }

        public string? Experience { get; set; }

        public string? Language { get; set; }

        public string? Skillsets { get; set; }
    }
}
