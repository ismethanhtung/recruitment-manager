using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;


namespace InsternShip.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public Task<JobListViewModel> GetAll(int page, int limit, string? search)
        {
            return _jobRepository.GetAll(page, limit, search);
        }
        
        public async Task<JobViewModel> GetById(Guid jobid)
        {
            return await _jobRepository.GetById(jobid);
        }
        public async Task<bool> Create(CreateJobModel job)
        {
            return await _jobRepository.Create(job);
        }
        public async Task<bool> Update(Guid id, JobUpdateModel job)
        {
            return await _jobRepository.Update(id, job);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _jobRepository.Delete(id);
        }
    }
}