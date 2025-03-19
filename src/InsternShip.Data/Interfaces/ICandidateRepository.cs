using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Interfaces
{
    public interface ICandidateRepository
    {
        Task<bool> Create(CreateCandidateModel request);
        Task<CandidateListViewModel> GetAll(string? search, int page, int limit, bool deleted);
        Task<CandidateViewModel> GetById(Guid? CandidateId, string? cv);
        Task<bool> Update(Guid? candidateId, CandidateUpdateModel request);
        Task<bool> Delete(Guid candidateId);
        Task<bool> Restore(Guid candidateId);
        Task<CandidateViewModel?> GetByUserId(Guid userId);
        Task<Guid?> GetIdByUserId(Guid userId);
        Task<string?> GetSkillSetsById(Guid? candidateId);
        Task<CandidateUpdateModel> GetMyCV(Guid? candidateId);
    }
}
