using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface IApplicationRepository
    {
        Task<InfoToInterviewModel> GetMailInfoByAppId(Guid appId);
        Task<IEnumerable<RecruitmentReportModel>> GetRecruitmentReport();
        Task<ApplicationListViewModel> GetAll(string? search, int page, int limit, string? statusDesc, Guid? candidateId, Guid? jobPostId, bool deleted);
        Task<ApplicationViewModel> GetById(Guid request);
        Task<bool> Create(ApplicationCreateModel request);
        Task<Guid> CreateGuid(ApplicationCreateModel request);
        Task<bool> Delete(Guid request);
        Task<bool> Restore(Guid request);
    }
}
