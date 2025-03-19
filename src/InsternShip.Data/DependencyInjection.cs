using InsternShip.Data.Interfaces;
using InsternShip.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InsternShip.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            service.AddTransient<IEventRepository, EventRepository>();
            service.AddTransient<IEventParticipationRepository, EventParticipationRepository>();
            service.AddTransient<IBlackListRepository, BlackListRepository>();
            service.AddTransient<IUserAccountRepository, UserAccountRepository>();
            service.AddTransient<IUserInfoRepository, UserInfoRepository>();
            service.AddTransient<ITestRepository, TestRepository>();
            service.AddTransient<IApplicationRepository, ApplicationRepository>();
            service.AddTransient<IQuestionRepository, QuestionRepository>();
            service.AddTransient<IApplicationStatusRepository, ApplicationStatusRepository>();
            service.AddTransient<IApplicationStatusUpdateRepository, ApplicationStatusUpdateRepository>();
            service.AddTransient<IQuestionBankRepository, QuestionBankRepository>();
            service.AddTransient<ICandidateRepository, CandidateRepository>();
            service.AddTransient<ICVRepository, CVRepository>();
            service.AddTransient<IRecruiterEventPostRepository, RecruiterEventPostRepository>();
            service.AddTransient<IRecruiterJobPostRepository, RecruiterJobPostRepository>();
            service.AddTransient<IRecruiterRepository, RecruiterRepository>();
            service.AddTransient<IJobRepository, JobRepository>();
            service.AddTransient<IInterviewerRepository, InterviewerRepository>();
            service.AddTransient<IInterviewRepository, InterviewRepository>();
            service.AddTransient<IMailerRepository, MailerRepository>();
            service.AddTransient<ICloudinaryRepository, CloudinaryRepository>();
            service.AddTransient<IUserRolesRepository, UserRolesRepository>();
            service.AddTransient<IDecodeRepository, DecodeRepository>();
            service.AddTransient<IInterviewSessionRepository, InterviewSessionRepository>();
            service.AddTransient<ISeedRepository, SeedRepository>();
            service.AddTransient<IExportDataRepository, ExportDataRepository>();
            service.AddTransient<IGetHtmlBodyRepository, GetHtmlBodyRepository>();
            service.AddTransient<IMeetingRepository, MeetingRepository>();
            service.AddTransient<IImportDataRepository, ImportDataRepository>();

            return service;
        }
    }
}
