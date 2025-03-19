using InsternShip.Data.Entities;
using InsternShip.Data.Model;

namespace InsternShip.Data.Interfaces
{
    public interface ICVRepository
    {
        Task<bool> Create(CVModel request);
        Task<bool> Delete(Guid? canId);
        Task<CV?> IsExistFile(Guid? candidateId);
        Task<CVModel?> GetByCanId(Guid? canId);


    }
}
