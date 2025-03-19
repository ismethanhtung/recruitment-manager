using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;


namespace InsternShip.Service
{
    public class BlackListService : IBlackListService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IBlackListRepository _blackListRepository;

        public BlackListService(IBlackListRepository blackListRepository, IUserAccountRepository userAccountRepository)
        {
            _blackListRepository = blackListRepository;
            _userAccountRepository = userAccountRepository;
        }
        public async Task<BlackListEntriesViewModel> GetAll(string? search, int page, int limit, bool isOn)
        {
            return await _blackListRepository.GetAll(search,page,limit, isOn);
        }
        public async Task<BlackListViewModel> GetById(Guid entryId)
        {
            return await _blackListRepository.GetById(entryId);
        }
        public async Task<bool> Create(CreateBlackListModel request)
        {
            _ = await _userAccountRepository.GetById(request.UserId);
            return await _blackListRepository.Create(request);
        }
        public async Task<bool> Update(Guid userId, BlackListUpdateModel request)
        {
            _ = await _userAccountRepository.GetById(userId);
            return await _blackListRepository.Update(userId, request);
        }
        public async Task<bool> Delete(Guid userId)
        {
            _ = await _userAccountRepository.GetById(userId);
            return await _blackListRepository.Delete(userId);
        }
        public async Task<bool> Restore(Guid userId)
        {
            _ = await _userAccountRepository.GetById(userId);
            return await _blackListRepository.Restore(userId);
        }

    }
}
