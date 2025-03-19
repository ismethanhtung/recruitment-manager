using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IApplicationStatusUpdateService
    {
        Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAll();
        Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllByApplicationId(Guid request);
        Task<ApplicationStatusUpdateViewModel> GetById(Guid request);
        Task<bool> Create(ApplicationStatusUpdateCreateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Update(ApplicationStatusUpdateModel request);
        Task<bool> UpdateStatus(string status, Guid applicationId);
    }
}
