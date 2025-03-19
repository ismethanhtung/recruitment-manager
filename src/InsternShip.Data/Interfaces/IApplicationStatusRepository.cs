using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IApplicationStatusRepository
    {
        Task<IEnumerable<ApplicationStatusViewModel>> GetAllApplicationStatus();
        Task<ApplicationStatusViewModel> GetById(Guid request);
        Task<ApplicationStatusModel> GetByDescription(string request);
        Task<bool> Create(ApplicationStatusCreateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Update(ApplicationStatusModel request);
    }
}
