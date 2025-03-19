using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public class RecruiterJobPostService : IRecruiterJobPostService
    {
        private readonly IRecruiterJobPostRepository _recruiterJobPostRepository;
        private readonly IRecruiterRepository _recruiterRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IInterviewRepository _interviewRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;

        public RecruiterJobPostService(IRecruiterJobPostRepository recruiterJobPostRepository, IRecruiterRepository recruiterRepository, IJobRepository jobRepository, IApplicationRepository applicationRepository, IInterviewRepository interviewRepository, IInterviewSessionRepository interviewSessionRepository, IApplicationStatusRepository applicationStatusRepository, IApplicationStatusUpdateRepository applicationStatusUpdateRepository)
        {
            _recruiterJobPostRepository = recruiterJobPostRepository;
            _recruiterRepository = recruiterRepository;
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _interviewRepository = interviewRepository;
            _interviewSessionRepository = interviewSessionRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _applicationStatusUpdateRepository  = applicationStatusUpdateRepository;
        }

        public async Task<bool> Create(Guid recId, CreateJobModel request)
        {

            _ = await _recruiterRepository.GetById(recId);
            var jobId = await _jobRepository.CreateGUID(request);
            var jp = new RecruiterJobPostModel()
            {
                RecruiterId = recId,
                JobId = jobId
            };
            return await _recruiterJobPostRepository.Create(jp);
        }

        public async Task<RecruiterJobPostListViewModel> GetAll(Guid? recId, int page, int limit, string? search, bool deleted)
        {
            return await _recruiterJobPostRepository.GetAll(recId, page, limit, search, deleted);
        }
        public async Task<RecruiterJobPostViewModel> GetById(Guid recPId)
        {
            return await _recruiterJobPostRepository.GetById(recPId);
        }

        public async Task<bool> Update(Guid postId, JobUpdateModel request)
        {
            var jobid = await _recruiterJobPostRepository.GetJobId(postId);

            return await _jobRepository.Update(jobid, request);
        }

        public async Task<bool> Delete(Guid postId)
        {
            return await _recruiterJobPostRepository.Delete(postId);
        }
        public async Task<bool> Restore(Guid postId)
        {
            return await _recruiterJobPostRepository.Restore(postId);
        }

        public async Task<IEnumerable<RecruiterPostedViewModel>> GetAllRecPosted()
        {
            return await _recruiterJobPostRepository.GetAllRecPosted();
        }

        public async Task<AppliedListViewModel> GetAllAppliedOfCandidate(Guid candidateId, int page, int limit, string? status)
        {
            var res = await _applicationRepository.GetAll(null, page, limit, status, candidateId,null,false);
            var infoList = new List<AppliedInfoViewModel>();
            foreach (var item in res.ApplicationList)
            {
                //item.
                var statusUpdate = await _applicationStatusUpdateRepository.GetByApplicationId(item.ApplicationId);
                _ = await _applicationStatusRepository.GetById(statusUpdate.StatusId);
                var jobPost = await _recruiterJobPostRepository.GetById(item.JobPostId);
                var info = new AppliedInfoViewModel()
                {
                    Application = item,
                    JobPost = jobPost,
                };
                infoList.Add(info);
            }
            return new AppliedListViewModel
            {
                AppliedList = infoList,
                TotalCount = res.TotalCount
            };
        }
        public async Task<OneRecruiterJobPostListViewModel> GetAllJobPostOfRecruiter(Guid recruiterId, int page, int limit)
        {
            var res = await _recruiterJobPostRepository.GetAll(recruiterId, page, limit, "",false);
            int appCount = 0, inteCount = 0;
            foreach(var item in res.JobPostList)
            {
                var app = await _applicationRepository.GetAll(null,page, limit, "",null, item.JobPostId, false);
                appCount++;
                foreach(var a in app.ApplicationList)
                {
                    var inte = await _interviewRepository.GetByApplicationId(a.ApplicationId);
                    if (inte != null)
                    {
                        var ses = await _interviewSessionRepository.GetAllSessionOfInterview(inte.InterviewId);
                        foreach (var session in ses)
                        {
                            inteCount++;
                        }
                    }
                }
            }
            return new OneRecruiterJobPostListViewModel
            {
                TotalCount = res.TotalCount,
                CandidateCount = appCount,
                InterviewerCount = inteCount,
                JobPostList = res.JobPostList,
            };
        }
    }
}
