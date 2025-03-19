using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace InsternShip.Data.Repositories
{
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<UserAccount> _userManager;
        public UserInfoRepository(RecruitmentDB context, IUnitOfWork uow, 
            IMapper mapper, UserManager<UserAccount> userManager) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<UserInfoViewModel> GetById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var infoUser = await Entities.FindAsync(user.InfoId) ??
                throw new KeyNotFoundException(ExceptionMessages.UserInforNotFound);
            var info = _mapper.Map<UserInfoViewModel>(infoUser);
            return info;
        }


        public async Task<bool> UpdateInfo(Guid userId, UserInfoUpdateModel newInfo)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new KeyNotFoundException(ExceptionMessages.UserNotFound);
            var infoUser = await Entities.FindAsync(user.InfoId) ??
                throw new KeyNotFoundException(ExceptionMessages.UserInforNotFound);
            try
            {
                _uow.BeginTransaction();
                var entry = Entities.Entry(infoUser);
                entry.CurrentValues.SetValues(newInfo);
                _uow.SaveChanges();
                _uow.CommitTransaction();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _uow.RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }


    }
}
