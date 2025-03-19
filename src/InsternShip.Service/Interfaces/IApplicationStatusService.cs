using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IApplicationStatusService
    {
        Task<IEnumerable<ApplicationStatusViewModel>> GetAll();
        Task<ApplicationStatusViewModel> GetById(Guid request);
        Task<bool> Create(ApplicationStatusCreateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Update(ApplicationStatusModel request);
    }
}
