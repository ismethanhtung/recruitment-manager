using InsternShip.Data.Model;

namespace InsternShip.Service.Interfaces
{
    public interface ICVService
    {
        Task<bool> Create(CreateCVModel request);
    }
}
