using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IJobService
    {

        Task<JobListViewModel> GetAll(int page, int limit, string? search);
        Task<JobViewModel> GetById(Guid jobid);
        Task<bool> Create(CreateJobModel job);
        Task<bool> Update(Guid id, JobUpdateModel job);
        Task<bool> Delete(Guid id);
    }
}