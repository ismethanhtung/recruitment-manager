using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationListViewModel> GetAll(string? search, int page, int limit, string? statusDesc, Guid? candidateId, Guid? jobPostId, bool deleted);
        Task<ApplicationViewModel> GetById(Guid request);
        Task<float> GetScoreByApplicationId(Guid request);
        Task<bool> Create(Guid canId, Guid jobPostId);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
    }
}
