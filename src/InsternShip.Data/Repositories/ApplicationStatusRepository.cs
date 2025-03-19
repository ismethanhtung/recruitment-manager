using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class ApplicationStatusRepository : Repository<ApplicationStatus>, IApplicationStatusRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ApplicationStatusRepository(RecruitmentDB dbContext, IUnitOfWork uow, IMapper mapper) : base(dbContext)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationStatusViewModel>> GetAllApplicationStatus()
        {
            try
            {
                var listData = new List<ApplicationStatusViewModel>();
                var data = await Entities.ToListAsync();
                foreach (var item in data)
                {
                    var obj = _mapper.Map<ApplicationStatusViewModel>(item);
                    listData.Add(obj);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationStatusViewModel> GetById(Guid request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.ApplicationStatusId == request)
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusNotExist);
            try
            {
                
                var obj = _mapper.Map<ApplicationStatusViewModel>(chosen);
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationStatusModel> GetByDescription(string request)
        {
            var chosen = await Entities.FirstOrDefaultAsync(x => x.Description == request)
            ??  throw new KeyNotFoundException(ExceptionMessages.StatusNotExist);

            try
            {
                var obj = _mapper.Map<ApplicationStatusModel>(chosen);
                return await Task.FromResult(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Create(ApplicationStatusCreateModel request)
        {
            try
            {
                var newApplication = _mapper.Map<ApplicationStatus>(request);
                Entities.Add(newApplication);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(ApplicationStatusModel request)
        {
            var chosen = await Entities.FindAsync(request.ApplicationStatusId)
                ?? throw new KeyNotFoundException(ExceptionMessages.StatusNotExist);
            try
            {

                _uow.BeginTransaction();
                var entry = Entities.Entry(chosen);
                entry.CurrentValues.SetValues(request);
                //Entities.Update(chosen);
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

        public async Task<bool> Delete(Guid request)
        {
            var chosen = await Entities.Include(x => x.ApplicationStatusUpdates).FirstOrDefaultAsync(y => y.ApplicationStatusId == request)
            ?? throw new KeyNotFoundException(ExceptionMessages.StatusNotExist);
            //else if (chosen.IsDeleted == true) throw new KeyNotFoundException(ExceptionMessages.StatusNotExist);

            try
            {
                Entities.Remove(chosen);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
