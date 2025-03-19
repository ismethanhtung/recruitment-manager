//using InsternShip.Data.Interfaces;
//using InsternShip.Data.Repositories;
//using InsternShip.Service.Interfaces;
using InsternShip.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InsternShip.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IBlackListService, BlackListService>();
            service.AddTransient<IUserAccountService, UserAccountService>();
            service.AddTransient<IUserInfoService, UserInfoService>();
            service.AddTransient<IEventService, EventService>();
            service.AddTransient<ITestService, TestService>();
            service.AddTransient<IEventParticipationService, EventParticipationService>();
            service.AddTransient<IQuestionService, QuestionService>();
            service.AddTransient<IApplicationService, ApplicationService>();
            service.AddTransient<IApplicationStatusService, ApplicationStatusService>();
            service.AddTransient<IApplicationStatusUpdateService, ApplicationStatusUpdateService>();
            service.AddTransient<IQuestionBankService, QuestionBankService>();
            service.AddTransient<ICandidateService, CandidateService>();
            service.AddTransient<ICVService, CVService>();
			service.AddTransient<IJobService, JobService>();
            service.AddTransient<IInterviewerService, InterviewerService>();
            service.AddTransient<IInterviewService, InterviewService>();
            service.AddTransient<IRecruiterService, RecruiterService>();
            service.AddTransient<IRecruiterJobPostService, RecruiterJobPostService>();
            service.AddTransient<IRecruiterEventPostService, RecruiterEventPostService>();
            service.AddTransient<IMailerService, MailerService>();
            service.AddTransient<IUserRoleService, UserRoleService>();
            service.AddTransient<IPermissionService, PermissionService>();
            service.AddTransient<ISeedService, SeedService>();
            service.AddTransient<IExportDataService, ExportDataService>();
            service.AddTransient<IInterviewSessionService, InterviewSessionService>();
            service.AddTransient<IMeetingService, MeetingService>();
            service.AddTransient<IImportDataService, ImportDataService>();

            return service;
        }
    }
}
