
namespace InsternShip.Data.Model
{
    public class MailRequestModel
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
    public class InfoToInterviewModel
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
    }
}
