using InsternShip.Data.Model;


namespace InsternShip.Service.Interfaces
{
    public interface IMailerService
    {
        Task<bool> SendEmail(MailRequestModel request);
        Task<bool> SendEmailSignUp(string Email, string token);
        Task<bool> SendEmailReset(string Email, string token);
        Task<bool> SendEmailInterview(CreateInterviewModel request);
        Task<bool> SendEmailInterview(Guid interviewerId,Guid interviewId);
        Task<bool> SendEmailAccept(Guid applicationId);
        Task<bool> SendEmailReject(Guid applicationId);
    }
}
