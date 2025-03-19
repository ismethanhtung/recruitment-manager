using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace InsternShip.Data.Repositories
{
    public class MailerRepository : IMailerRepository
    {
        private readonly IConfiguration _config;

        public MailerRepository (IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmail(MailRequestModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            return await Task.FromResult(true);
        }
    }
}
