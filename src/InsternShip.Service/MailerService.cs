using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class MailerService : IMailerService
    {
        private readonly IMailerRepository _mailerRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IGetHtmlBodyRepository _getHtmlBodyRepository;
        private readonly IRecruiterJobPostRepository _jobRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        public MailerService(IMailerRepository mailerRepository, IApplicationRepository applicationRepository, IInterviewerRepository interviewerRepository, IInterviewRepository interviewRepository, IGetHtmlBodyRepository getHtmlBodyRepository, IRecruiterJobPostRepository jobRepository, IRecruiterRepository recruiterRepository)
        {
            _mailerRepository = mailerRepository;
            _applicationRepository = applicationRepository;
            _interviewerRepository = interviewerRepository;
            _getHtmlBodyRepository = getHtmlBodyRepository;
            _interviewRepository = interviewRepository;
            _jobRepository = jobRepository;
            _recruiterRepository = recruiterRepository;
        }
        public async Task<bool> SendEmail(MailRequestModel request)
        {
            return await _mailerRepository.SendEmail(request);
        }
        private DateTime ToVietnameseTime(DateTime dateTime)
        {
            DateTime time = dateTime;
            TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(time, systemTimeZone, vietnamTimeZone);
            return vietnamTime;

        }
        public async Task<bool> SendEmailSignUp(string email, string token)
        {
            var verifLink = "https://hcm23net03gr01-001-site1.htempurl.com" + token;
            var body = await _getHtmlBodyRepository.GetBody("verify.html");

            body = body.Replace("[[verilink]]", verifLink);
            var confirmationMail = new MailRequestModel
            {
                To = email,
                Subject = "Verified your email",
                Body = body
            };
            return await _mailerRepository.SendEmail(confirmationMail);
        }
        public async Task<bool> SendEmailReset(string email, string token)
        {
            var verifLink = "https://hcm23net03gr01-001-site1.htempurl.com" + token;
            var body = await _getHtmlBodyRepository.GetBody("forgot_password.html");
            body = body.Replace("[[verilink]]", verifLink);
            var confirmationMail = new MailRequestModel
            {
                To = email,
                Subject = "Reset Password",
                Body = body
            };
            return await _mailerRepository.SendEmail(confirmationMail);
        }
        public async Task<bool> SendEmailInterview(CreateInterviewModel request)
        {
            var info = await _applicationRepository.GetMailInfoByAppId(request.ApplicationId);
            var body = await _getHtmlBodyRepository.GetBody("interview.html");

            body = body.Replace("[[FirstName]]", info.FirstName);
            body = body.Replace("[[Location]]", request.Location);
            //DateTime vietnamTime = ToVietnameseTime((DateTime)request.StartTime); 
            body = body.Replace("[[StartTime]]", ((DateTime)request.StartTime).ToString("dddd, dd MMMM yyyy, hh:mm tt"));

            var mailReq = new MailRequestModel
            {
                To = info.Email,
                Subject = "Interview Invitation",
                Body = body

            };
            return await _mailerRepository.SendEmail(mailReq);
        }
        public async Task<bool> SendEmailInterview(Guid interviewerId, Guid interviewId)
        {
            var info = await _interviewerRepository.GetById(interviewerId);
            var interview = await _interviewRepository.GetById(interviewId);
            var body = await _getHtmlBodyRepository.GetBody("interview.html");

            body = body.Replace("[[FirstName]]", info.FirstName);
            body = body.Replace("[[Location]]", interview.Location);
            //DateTime vietnamTime = ToVietnameseTime((DateTime)interview.StartTime);
            body = body.Replace("[[StartTime]]", ((DateTime)interview.StartTime).ToString("dddd, dd MMMM yyyy, hh:mm tt"));

            var mailReq = new MailRequestModel
            {
                To = info.Email,
                Subject = "Interview Invitation",
                Body = body

            };
            return await _mailerRepository.SendEmail(mailReq);
        }
        public async Task<bool> SendEmailAccept(Guid applicationId)
        {
            var application = await _applicationRepository.GetById(applicationId);
            var job = await _jobRepository.GetById(application.JobPostId);
            var reccer = await _recruiterRepository.GetById(job.RecruiterId);
            var body = await _getHtmlBodyRepository.GetBody("accept.html");

            body = body.Replace("[[FirstName]]", application.CandidateName);
            body = body.Replace("[[JobName]]", job.JobName);
            body = body.Replace("[[Email]]", reccer.Email);
            body = body.Replace("[[Time]]", (DateTime.Now.AddDays(15)).ToString("dddd, dd MMMM yyyy"));

            var mailReq = new MailRequestModel
            {
                To = application.Email,
                Subject = "Application Notice",
                Body = body

            };
            return await _mailerRepository.SendEmail(mailReq);
        }
        public async Task<bool> SendEmailReject(Guid applicationId)
        {
            var application = await _applicationRepository.GetById(applicationId);
            var job = await _jobRepository.GetById(application.JobPostId);
            var reccer = await _recruiterRepository.GetById(job.RecruiterId);
            var body = await _getHtmlBodyRepository.GetBody("reject.html");

            body = body.Replace("[[FirstName]]", application.CandidateName);
            body = body.Replace("[[JobName]]", job.JobName);
            body = body.Replace("[[Email]]", reccer.Email);

            var mailReq = new MailRequestModel
            {
                To = application.Email,
                Subject = "Application Notice",
                Body = body

            };
            return await _mailerRepository.SendEmail(mailReq);
        }
    }
}
