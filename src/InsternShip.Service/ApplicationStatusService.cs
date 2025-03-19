using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class ApplicationStatusService : IApplicationStatusService
    {
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        public ApplicationStatusService(IApplicationStatusRepository applicationStatusRepository)
        {
            _applicationStatusRepository = applicationStatusRepository;
        }
        public async Task<IEnumerable<ApplicationStatusViewModel>> GetAll()
        {
            return await _applicationStatusRepository.GetAllApplicationStatus();
        }

        public async Task<ApplicationStatusViewModel> GetById(Guid request)
        {
            return await _applicationStatusRepository.GetById(request);
        }

        public async Task<bool> Delete(Guid request)
        {
            return await _applicationStatusRepository.Delete(request);
        }

        public async Task<bool> Create(ApplicationStatusCreateModel request)
        {
            return await _applicationStatusRepository.Create(request);
        }

        public async Task<bool> Update(ApplicationStatusModel request)
        {
            return await _applicationStatusRepository.Update(request);
        }
    }
}
