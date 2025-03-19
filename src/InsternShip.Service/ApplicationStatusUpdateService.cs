using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class ApplicationStatusUpdateService : IApplicationStatusUpdateService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        public ApplicationStatusUpdateService(IApplicationStatusUpdateRepository applicationStatusUpdateRepository, IApplicationRepository applicationRepository, IApplicationStatusRepository applicationStatusRepository)
        {
            _applicationStatusUpdateRepository = applicationStatusUpdateRepository;
            _applicationRepository = applicationRepository;
            _applicationStatusRepository = applicationStatusRepository;
        }
        public async Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAll()
        {
            return await _applicationStatusUpdateRepository.GetAllApplicationStatusUpdate();
        }

        public async Task<IEnumerable<ApplicationStatusUpdateViewModel>> GetAllByApplicationId(Guid request)
        {
            return await _applicationStatusUpdateRepository.GetAllByApplicationId(request);
        }

        public async Task<ApplicationStatusUpdateViewModel> GetById(Guid request)
        {
            return await _applicationStatusUpdateRepository.GetById(request);
        }

        public async Task<bool> Delete(Guid request)
        {
            return await _applicationStatusUpdateRepository.Delete(request);
        }

        public async Task<bool> Create(ApplicationStatusUpdateCreateModel request)
        {
            return await _applicationStatusUpdateRepository.Create(request);
        }

        public async Task<bool> Update(ApplicationStatusUpdateModel request)
        {
            return await _applicationStatusUpdateRepository.Update(request);
        }

        public async Task<bool> UpdateStatus(string status, Guid applicationId)
        {
            var chosenApplication = await _applicationRepository.GetById(applicationId);
            var chosenStatus = await _applicationStatusRepository.GetByDescription(status);
            
            var newStatus = new ApplicationStatusUpdateCreateModel
                {
                    ApplicationId = chosenApplication.ApplicationId,
                    StatusId = chosenStatus.ApplicationStatusId,
                };
                return await _applicationStatusUpdateRepository.Create(newStatus);
        }
    }
}
