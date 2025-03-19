using InsternShip.Data.Model;

namespace InsternShip.Data.Interfaces
{
    public interface IMailerRepository
    {
        Task<bool> SendEmail(MailRequestModel request);

    }
}