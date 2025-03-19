using AutoMapper;
using InsternShip.Data.Entities;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InsternShip.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Recruiter/JobPost/EventPost
            CreateMap<Job, RecruiterJobPostViewModel>();

            CreateMap<Recruiter, RecruiterJobPostViewModel>();

            CreateMap<RecruiterJobPost, RecruiterJobPostViewModel>()

                .ForMember(destination => destination.FirstName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.FirstName))
                .ForMember(destination => destination.LastName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.LastName))
                /*.ForMember(destination => destination.Level,
                    options => options.MapFrom(source => source.Job.JobLevel.Name))*/
                .ForMember(destination => destination.JobName,
                    options => options.MapFrom(source => source.Job.Name))
                .ForMember(destination => destination.TypeName,
                    options => options.MapFrom(source => source.Job.Type))
                .IncludeMembers(source => source.Recruiter)
                .IncludeMembers(source => source.Job);

            CreateMap<Recruiter, RecruiterPostedViewModel>();
            CreateMap<RecruiterJobPost, RecruiterPostedViewModel>()
                .ForMember(destination => destination.FirstName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.FirstName))
                .ForMember(destination => destination.LastName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.LastName))
                .IncludeMembers(source => source.Recruiter);

            CreateMap<Recruiter, RecruiterEventPostViewModel>();
            CreateMap<Event, RecruiterEventPostViewModel>();
            CreateMap<Event, EventViewModel>();
            CreateMap<CreateEventModel, Event>();
            CreateMap<EventParticipationCreateModel, EventParticipation>();
            CreateMap<EventParticipation, EventParticipationViewModel>();
            CreateMap<RecruiterEventPost, RecruiterEventPostViewModel>()
                .ForMember(destination => destination.FirstName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.FirstName))
                .ForMember(destination => destination.LastName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.LastName))
                .ForMember(destination => destination.EventName,
                    options => options.MapFrom(source => source.Event.Name))
                .IncludeMembers(source => source.Recruiter)
                .IncludeMembers(source => source.Event);

            CreateMap<RecruiterJobPostModel, RecruiterJobPost>();

            CreateMap<RecruiterEventPostModel, RecruiterEventPost>();

            CreateMap<Recruiter, RecruiterPostedViewModel>();
            CreateMap<RecruiterEventPost, RecruiterPostedViewModel>()
                .ForMember(destination => destination.FirstName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.FirstName))
                .ForMember(destination => destination.LastName,
                    options => options.MapFrom(source => source.Recruiter.UserAccount.UserInfo.LastName))
                .IncludeMembers(source => source.Recruiter);

            CreateMap<Recruiter, RecruiterViewModel>()
                .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.FirstName))
                .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.LastName))
                .ForMember(destination => destination.Email,
                   options => options.MapFrom(source => source.UserAccount.Email))
                .ForMember(destination => destination.PhoneNumber,
                   options => options.MapFrom(source => source.UserAccount.PhoneNumber))
                .ForMember(destination => destination.Avatar,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.Avatar));
            CreateMap<CreateRecruiterModel, Recruiter>()
                .ForMember(destination => destination.UserId,
                    options => options.MapFrom(source => source.UserId));
            #endregion

            #region User
            CreateMap<UserInfo, UserInfoViewModel>();

            CreateMap<UserAccount, UserAccountViewModel>()
                .ForMember(destination => destination.UserId,
                    options => options.MapFrom(source => source.Id))
                .ForMember(destination => destination.Email,
                    options => options.MapFrom(source => source.Email))
                .ForMember(destination => destination.UserName,
                    options => options.MapFrom(source => source.UserName));
            #endregion

            #region Job

            CreateMap<Job, JobViewModel>();
            CreateMap<JobViewModel, Job>();
            CreateMap<CreateJobModel, Job>();
            CreateMap<Job, CreateJobModel>();
            //CreateMap<Job, JobViewModel>();
            #endregion

            #region Application
            CreateMap<Application, ApplicationViewModel>()
                .ForMember(destination => destination.CandidateId,
                    option => option.MapFrom(source => source.Candidate.CandidateId))
                .ForMember(destination => destination.CandidateName,
                    option => option.MapFrom(source => source.Candidate.UserAccount.UserInfo.FirstName + " " + source.Candidate.UserAccount.UserInfo.LastName))
                .ForMember(destination => destination.Email,
                    option => option.MapFrom(source => source.Candidate.UserAccount.Email))
                .ForMember(destination => destination.PhoneNumber,
                    option => option.MapFrom(source => source.Candidate.UserAccount.PhoneNumber))
                .ForMember(destination => destination.AppliedPosition,
                    option => option.MapFrom(source => source.RecruiterJobPosts.Job.Name));

            CreateMap<Application, InfoToInterviewModel>()
                .ForMember(destination => destination.Email,
                    option => option.MapFrom(source => source.Candidate.UserAccount.Email))
                .ForMember(destination => destination.FirstName,
                    option => option.MapFrom(source => source.Candidate.UserAccount.UserInfo.FirstName));

            CreateMap<ApplicationCreateModel, Application>();
            CreateMap<ApplicationStatus, ApplicationStatusViewModel>();
            CreateMap<ApplicationStatusModel, ApplicationStatus>();
            CreateMap<ApplicationStatus, ApplicationStatusModel>();
            CreateMap<ApplicationStatusCreateModel, ApplicationStatus>();


            CreateMap<ApplicationStatusUpdate, ApplicationStatusUpdateViewModel>();
            CreateMap<ApplicationStatusUpdate, ApplicationStatusUpdateModel>();
            CreateMap<ApplicationStatusUpdateCreateModel, ApplicationStatusUpdate>();
            #endregion

            #region Question
            CreateMap<Question, QuestionViewModel>()
                .ForMember(destination => destination.Tag,
                    options => options.MapFrom(source => source.Tag))
                .ForMember(destination => destination.Level,
                    options => options.MapFrom(source => source.Level));
            CreateMap<CreateQuestionModel, Question>()
                .ForMember(destination => destination.Detail,
                    options => options.MapFrom(source => source.Detail))
                .ForMember(destination => destination.MaxScore,
                    options => options.MapFrom(source => source.MaxScore))
                .ForMember(destination => destination.Note,
                    options => options.MapFrom(source => source.Note))
                .ForMember(destination => destination.Tag,
                    options => options.MapFrom(source => source.Tag))
                .ForMember(destination => destination.Level,
                    options => options.MapFrom(source => source.Level));
            CreateMap<Question, QuestionModel>()
                .ForMember(destination => destination.QuestionId,
                    options => options.MapFrom(source => source.QuestionId))
                .ForMember(destination => destination.Detail,
                    options => options.MapFrom(source => source.Detail))
                .ForMember(destination => destination.Detail,
                    options => options.MapFrom(source => source.MaxScore))
                .ForMember(destination => destination.Note,
                    options => options.MapFrom(source => source.Note))
                .ForMember(destination => destination.Tag,
                    options => options.MapFrom(source => source.Tag))
                .ForMember(destination => destination.Level,
                    options => options.MapFrom(source => source.Level));

            CreateMap<Question, QuestionUpdateModel>()
                /*.ForMember(destination => destination.QuestionId,
                    options => options.MapFrom(source => source.QuestionId))*/
                .ForMember(destination => destination.Detail,
                    options => options.MapFrom(source => source.Detail))
                .ForMember(destination => destination.Detail,
                    options => options.MapFrom(source => source.MaxScore))
                .ForMember(destination => destination.Note,
                    options => options.MapFrom(source => source.Note));
            #endregion

            #region Test
            CreateMap<Test, TestViewModel>()
                .ForMember(destination => destination.TotalScore,
                    options => options.MapFrom(source => source.TotalScore))
                .ForMember(destination => destination.StartTime,
                    options => options.MapFrom(source => source.StartTime))
                .ForMember(destination => destination.EndTime,
                    options => options.MapFrom(source => source.EndTime));
            CreateMap<Test, TestModel>();

            CreateMap<CreateTestModel, Test>();
            CreateMap<Test, TestUpdateModel>();
            #endregion

            #region Blacklist
            CreateMap<BlackList, BlackListViewModel>()
               .ForMember(destination => destination.BlacklistId,
                   options => options.MapFrom(source => source.BlackListId))
               .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.FirstName))
               .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.LastName))
               .ForMember(destination => destination.UserId,
                   options => options.MapFrom(source => source.UserAccount.Id))
               .ForMember(destination => destination.Reason,
                   options => options.MapFrom(source => source.Reason))
               .ForMember(destination => destination.Duration,
                   options => options.MapFrom(source => source.Duration))
               .ForMember(destination => destination.EntryDate,
                   options => options.MapFrom(source => source.EntryDate));
            CreateMap<BlackList, BlackListUpdateModel>();
            CreateMap<CreateBlackListModel, BlackList>();
            #endregion

            #region Candidate
            CreateMap<Candidate, CandidateViewModel>()
            .ForMember(destination => destination.CandidateId,
                   options => options.MapFrom(source => source.CandidateId))
            .ForMember(destination => destination.UserId,
                   options => options.MapFrom(source => source.UserAccount.Id))
            .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.FirstName))
            .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.LastName))
            .ForMember(destination => destination.Email,
                   options => options.MapFrom(source => source.UserAccount.Email))
            .ForMember(destination => destination.PhoneNumber,
                   options => options.MapFrom(source => source.UserAccount.PhoneNumber))
            .ForMember(destination => destination.Education,
                   options => options.MapFrom(source => source.Education))
            .ForMember(destination => destination.Skillsets,
                   options => options.MapFrom(source => source.Skillsets))
            .ForMember(destination => destination.Description,
                   options => options.MapFrom(source => source.Description))
            .ForMember(destination => destination.Experience,
                   options => options.MapFrom(source => source.Experience))
            .ForMember(destination => destination.Language,
                   options => options.MapFrom(source => source.Language))
            .ForMember(destination => destination.Avatar,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.Avatar));
            CreateMap<Candidate, CandidateUpdateModel>();
            CreateMap<CreateCandidateModel, Candidate>();
            #endregion

            #region CV
            CreateMap<CVModel, CV>().ReverseMap();
            #endregion

            #region Role

            CreateMap<Roles, RoleViewModel>()
                .ForMember(destination => destination.RoleId,
                    option => option.MapFrom(source => source.Id));

            CreateMap<RoleViewModel, Roles>()
                .ForMember(destination => destination.Id,
                    option => option.MapFrom(source => source.RoleId));

            CreateMap<RoleModel, Roles>();

            CreateMap<IdentityRoleClaim<Guid>, RoleClaimsViewModel>()
                .ForMember(destination => destination.ClaimValue,
                   options => options.MapFrom(source => source.ClaimType))
            .ForMember(destination => destination.RoleClaimsId,
                   options => options.MapFrom(source => source.Id));

            CreateMap<RoleClaimsModel, RoleClaims>()
                .ForMember(destination => destination.ClaimValue,
                   options => options.MapFrom(source => source.ClaimType));

            CreateMap<UserRolesModel, UserRoles>();

            #endregion

            #region Interviewer
            CreateMap<Interviewer, InterviewerViewModel>()
            .ForMember(destination => destination.InterviewerId,
                   options => options.MapFrom(source => source.InterviewerId))
            .ForMember(destination => destination.UserId,
                   options => options.MapFrom(source => source.UserAccount.Id))
            .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.FirstName))
            .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.LastName))
            .ForMember(destination => destination.Email,
                   options => options.MapFrom(source => source.UserAccount.Email))
            .ForMember(destination => destination.PhoneNumber,
                   options => options.MapFrom(source => source.UserAccount.PhoneNumber))
            .ForMember(destination => destination.UrlContact,
                   options => options.MapFrom(source => source.UrlContact))
            .ForMember(destination => destination.Avatar,
                   options => options.MapFrom(source => source.UserAccount.UserInfo.Avatar));
            CreateMap<Interviewer, InterviewerUpdateModel>();
            CreateMap<CreateInterviewerModel, Interviewer>();
            #endregion

            #region Interview
            CreateMap<Interview, InterviewViewModel>()
            .ForMember(destination => destination.JobPostId,
                   options => options.MapFrom(source => source.Application.JobPostId))
            .ForMember(destination => destination.JobName,
                   options => options.MapFrom(source => source.Application.RecruiterJobPosts.Job.Name))
            .ForMember(destination => destination.CandidateName,
                   options => options.MapFrom(source =>
                   source.Application.Candidate.UserAccount.UserInfo.FirstName + " " +
                   source.Application.Candidate.UserAccount.UserInfo.LastName));
            CreateMap<Interview, InterviewUpdateModel>();
            CreateMap<CreateInterviewModel, Interview>();
                
            #endregion

            #region EventParticipation
            CreateMap<Candidate, EventParticipationViewModel>();
            CreateMap<EventParticipation, EventParticipationViewModel>()
            .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.Candidate.UserAccount.UserInfo.FirstName))
            .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.Candidate.UserAccount.UserInfo.LastName))
            .ForMember(destination => destination.Email,
                   options => options.MapFrom(source => source.Candidate.UserAccount.Email))
            .ForMember(destination => destination.PhoneNumber,
                   options => options.MapFrom(source => source.Candidate.UserAccount.PhoneNumber))
            .ForMember(destination => destination.Education,
                   options => options.MapFrom(source => source.Candidate.Education))
            .ForMember(destination => destination.Skillsets,
                   options => options.MapFrom(source => source.Candidate.Skillsets))
            .ForMember(destination => destination.Description,
                   options => options.MapFrom(source => source.Candidate.Description))
            .ForMember(destination => destination.Experience,
                   options => options.MapFrom(source => source.Candidate.Experience))
            .ForMember(destination => destination.Language,
                   options => options.MapFrom(source => source.Candidate.Language))
            .IncludeMembers(source => source.Candidate);

            CreateMap<RecruiterEventPost, EventPostParticipationViewModel>();
            CreateMap<EventParticipation, EventPostParticipationViewModel>()
            .ForMember(destination => destination.FirstName,
                   options => options.MapFrom(source => source.RecruiterEventPost.Recruiter.UserAccount.UserInfo.FirstName))
            .ForMember(destination => destination.LastName,
                   options => options.MapFrom(source => source.RecruiterEventPost.Recruiter.UserAccount.UserInfo.LastName))
            .ForMember(destination => destination.EventName,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.Name))
            .ForMember(destination => destination.Location,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.Location))
            .ForMember(destination => destination.MaxCandidate,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.MaxCandidate))
            .ForMember(destination => destination.Description,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.Description))
            .ForMember(destination => destination.Poster,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.Poster))
            .ForMember(destination => destination.Online,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.Status))
            .ForMember(destination => destination.Approved,
                   options => options.MapFrom(source => source.Status))
            .ForMember(destination => destination.StartDate,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.StartDate))
            .ForMember(destination => destination.EndDate,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.EndDate))
            .ForMember(destination => destination.PostDate,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.PostDate))
            .ForMember(destination => destination.DeadlineDate,
                   options => options.MapFrom(source => source.RecruiterEventPost.Event.DeadlineDate))
            .IncludeMembers(source => source.RecruiterEventPost);


            #endregion

            #region InterviewSession
            CreateMap<InterviewSession, InterviewSessionViewModel>()
                .ForMember(destination => destination.SessionId,
                    options => options.MapFrom(source => source.InterviewSessionId))
                .ForMember(destination => destination.TestId,
                    options => options.MapFrom(source => source.TestId))
                .ForMember(destination => destination.CandidateName,
                    option => option.MapFrom(source => source.Interview.Application.Candidate.UserAccount.UserInfo.FirstName + " " + source.Interview.Application.Candidate.UserAccount.UserInfo.LastName))
                .ForMember(destination => destination.Location,
                    options => options.MapFrom(source => source.Interview.Location))
                .ForMember(destination => destination.GivenScore,
                    options => options.MapFrom(source => source.GivenScore))
                .ForMember(destination => destination.Note,
                    options => options.MapFrom(source => source.Note));

            CreateMap<InterviewSession, UpdatedInterviewSessionModel>();
            CreateMap<CreateInterviewSessionModel, InterviewSession>()
                .ForMember(destination => destination.InterviewId,
                    options => options.MapFrom(source => source.InterviewId))
                .ForMember(destination => destination.InterviewerId,
                    options => options.MapFrom(source => source.InterviewerId));
            #endregion
        }
    }
}
