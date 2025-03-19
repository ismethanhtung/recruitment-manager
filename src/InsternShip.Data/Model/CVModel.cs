using Microsoft.AspNetCore.Http;

namespace InsternShip.Data.Model
{
    public class CVModel
    {
        public Guid? CandidateId { get; set; }
        public string? UrlFile { get; set; }
        public string? PublicIdFile { get; set; }
    }

    public class CreateFileCVModel
    {
        public IFormFile File { get; set; }
    }

    public class CreateCVModel
    {
        public Guid CurrentId { get; set; }
        public IFormFile File { get; set; }
    }
}
