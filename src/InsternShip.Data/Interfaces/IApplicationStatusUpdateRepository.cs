using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IApplicationStatusUpdateRepository
    {
        Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllApplicationStatusUpdate();
        Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllByApplicationId(Guid request);
        Task<ApplicationStatusUpdateViewModel> GetByApplicationIdAndStatus(Guid applicationRequest, Guid statusRequest);
        Task<ApplicationStatusUpdateViewModel> GetById(Guid request);
        Task<ApplicationStatusUpdateModel> GetByApplicationId(Guid request);
        Task<bool> AcceptApplication(Guid appid, Guid recId, Guid statusId);
        Task<bool> RejectApplication(Guid appid, Guid recId, Guid statusId);
        Task<bool> Create(ApplicationStatusUpdateCreateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Update(ApplicationStatusUpdateModel request);
        Task<IEnumerable<CandidateReportModel>> GetCandidateReport();

    }
}
